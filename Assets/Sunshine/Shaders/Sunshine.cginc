// Upgrade NOTE: replaced 'UNITY_PASS_TEXCUBE(unity_SpecCube1)' with 'UNITY_PASS_TEXCUBE_SAMPLER(unity_SpecCube1,unity_SpecCube0)'

//Required for some things...
#include "UnityCG.cginc"

// ============================== CONSTANTS ==============================

#ifdef SUNSHINE_MOBILE
	#define SUNSHINE_ONE_CASCADE
	#ifndef SUNSHINE_OVERCAST_ON
		#undef SUNSHINE_OVERCAST_OFF
		#define SUNSHINE_OVERCAST_OFF
	#endif
	#ifndef SUNSHINE_TEXCENTERING_ON
		#undef SUNSHINE_TEXCENTERING_OFF
		#define SUNSHINE_TEXCENTERING_OFF
	#endif
#endif

#define PRE_TRANSFORM_INTO_CASCADE0

//PI is a lie ;)
#define TAU 6.2831853072
#define HALF_TAU 3.1415926536

#ifdef UNITY_COMPILER_HLSL
	#define SUNSHINE_INITIALIZE_OUTPUT(t, v) v = (t)0;
#else
	#define SUNSHINE_INITIALIZE_OUTPUT(t, v)
#endif

#ifdef UNITY_SAMPLE_1CHANNEL
	#define SUNSHINE_SAMPLE_1CHANNEL(t, c) UNITY_SAMPLE_1CHANNEL(t, c)
#else
	#define SUNSHINE_SAMPLE_1CHANNEL(t, c) tex2D(t, c).x
#endif


#if defined(SHADER_TARGET_GLSL)
	#define SUNSTEP(y, x) ((y <= x) ? 1.0 : 0.0)
#else
	#define SUNSTEP(y, x) step(y, x)
#endif


// ============================== PARAMETERS ==============================

sampler2D sunshine_Lightmap;
sampler2D sunshine_OvercastMap;

float4 sunshine_Lightmap_TexelSize; //Use the built-in functionality. :)
#define sunshine_LightmapSize (sunshine_Lightmap_TexelSize.zw)
#define sunshine_LightmapTexel (sunshine_Lightmap_TexelSize.xy)

float4 sunshine_ShadowParamsAndHalfTexel;
#define sunshine_ShadowParams (sunshine_ShadowParamsAndHalfTexel.xy)
#define sunshine_LightmapHalfTexel (sunshine_ShadowParamsAndHalfTexel.zw)

float2 sunshine_OvercastCoord;
float4 sunshine_OvercastVectorsUV;

float4x4 sunshine_CameraVToSunVP;
float4x4 sunshine_WorldToSunVP;

float4 sunshine_ShadowCoordDepthStart;
float4 sunshine_ShadowCoordDepthRayZ;
float4 sunshine_ShadowCoordDepthRayU;
float4 sunshine_ShadowCoordDepthRayV;
float3 sunshine_ShadowToWorldScale;

float4 sunshine_CascadeNearRatiosSq;
float4 sunshine_CascadeFarRatiosSq;

// Assuming the cascades are square, we can use .xx and .y can be the z-offset...
// This allows staggared cascade rendering without adjusting the depth texture!
float4x4 sunshine_CascadeRanges;

float3 sunshine_ShadowFadeParams;


// ============================== MATH HELPERS ==============================

float2 SnapCoord(float2 TexCoord, float2 Pixel)
{
	return floor(TexCoord * (float2(1, 1) / Pixel)) * Pixel;
}

float2x2 RotationMatrix2D(float r)
{
    float c = cos(r);
    float s = sin(r);
	return float2x2(c, -s, s, c);
}
float2 RotationRay2D(float r)
{
	return float2(cos(r), -sin(r));
}



// Input: Texture Coordinate
// Returns [0, 1]
float Random(float2 coord)
{
	//better noise attempt...
	//return frac(dot(sin(coord / float2x2(1,9999,0,1)) * float2x2(9999,0,0,1), 1));
	return frac(sin(dot(coord ,float2(78.2331, 120.98981))) * 43758.5453);
}

#define Pulse(dist) frac((dist) * 123456789.987654321)
#define PulseX(dist, pulseCount) frac((dist) * (pulseCount))

// ============================== SHADOW HELPERS ==============================


// Encoding/decoding [0..1) floats into 8 bit/channel RGBA. Note that 1.0 will not be encoded properly.
inline float4 SunshineEncodeFloatRGBA( float v )
{
	float4 kEncodeMul = float4(1.0, 255.0, 65025.0, 16581375.0); //Was 160581375, but 16581375 seems correct...?
	float kEncodeBit = 1.0/255.0;
	float4 enc = kEncodeMul * v;
	enc = frac (enc);
	enc -= enc.yzww * kEncodeBit;
	return enc;
}
inline float SunshineDecodeFloatRGBA( float4 enc )
{
	float4 kDecodeDot = float4(1.0, 1/255.0, 1/65025.0, 1/16581375.0); //Was 1/160581375, but 1/16581375 seems correct...?
	return dot( enc, kDecodeDot );
}


#define WriteSun(depth) SunshineEncodeFloatRGBA(depth)
//#define SampleSun(texCoord) DecodeFloatRGBA(tex2D(sunshine_Lightmap, (texCoord)))
//Paranoid about inlined DecodeFloatRGBA() producing 2x tex2D() calls, probably unfounded:
inline float SampleSun(float2 t)
{
	return SunshineDecodeFloatRGBA(tex2D(sunshine_Lightmap, (t)));
}

#ifndef OverrideSunshineShadowTerm
	#ifdef SUNSHINE_FILTER_HARD
		#define OverrideSunshineShadowTerm(shadowCoordDepth) ShadowTermHard(shadowCoordDepth)
	#endif
	#ifdef SUNSHINE_FILTER_PCF_2x2
		#define OverrideSunshineShadowTerm(shadowCoordDepth) ShadowTermPCF2x2(shadowCoordDepth)
	#endif
	#ifdef SUNSHINE_FILTER_PCF_3x3
		#define OverrideSunshineShadowTerm(shadowCoordDepth) ShadowTermPCF3x3(shadowCoordDepth)
	#endif
	#ifdef SUNSHINE_FILTER_PCF_4x4
		#define OverrideSunshineShadowTerm(shadowCoordDepth) ShadowTermPCF4x4(shadowCoordDepth)
	#endif
#endif

//Failsafe Hard Shadows:
#ifndef OverrideSunshineShadowTerm
	#define OverrideSunshineShadowTerm(shadowCoordDepth) ShadowTermHard(shadowCoordDepth)
#endif

inline float OvercastTerm(float2 lightCoord) { return tex2D(sunshine_OvercastMap, sunshine_OvercastCoord.xy + sunshine_OvercastVectorsUV.xy * lightCoord.x + sunshine_OvercastVectorsUV.zw * lightCoord.y).a; }

#ifdef SUNSHINE_NO_FADE
	#define SunshineFadeSaturate(v) SUNSTEP(0, v)
#else
	#define SunshineFadeSaturate(v) saturate(v)
#endif

//Normal Version:
#define SunshineShadowTerm(lightCoordDepthFade) (1.0 - OverrideSunshineShadowTerm(lightCoordDepthFade.xyz) * sunshine_ShadowParams.x * SunshineFadeSaturate(lightCoordDepthFade.w))

//Overcast Version
#define SunshineShadowTermOvercast(lightCoordDepthFade, overcastTerm) (1.0 - (1.0 - (1.0 - OverrideSunshineShadowTerm(lightCoordDepthFade.xyz) * SunshineFadeSaturate(lightCoordDepthFade.w)) * (overcastTerm)) * sunshine_ShadowParams.x)

#ifdef SUNSHINE_ONE_CASCADE
	#define SunshineInCascades(depth01) float4(1,0,0,0)
#else
	#define SunshineInCascades(depth01) (SUNSTEP(sunshine_CascadeNearRatiosSq, depth01) * SUNSTEP(depth01, sunshine_CascadeFarRatiosSq))
#endif
// Assuming the cascades are square, we can use .xx and .y can be the z-offset...
// This allows staggared cascade rendering without adjusting the depth texture!
//SUNSHINE_ONE_CASCADE and SUNSHINE_TWO_CASCADES are optional optimizations...


#if defined(PRE_TRANSFORM_INTO_CASCADE0) && defined(SUNSHINE_ONE_CASCADE)
	float2 CalculateSunCoord(float3 shadowCoordDepth) { return shadowCoordDepth.xy; }
#else
	float2 CalculateSunCoord(float3 shadowCoordDepth)
	{
		//No Constant Waterfalling!...
		float4 inSplits = SunshineInCascades(shadowCoordDepth.z);
		float4 cascadeRange = sunshine_CascadeRanges[0]
		#ifndef SUNSHINE_ONE_CASCADE
			* inSplits[0] + sunshine_CascadeRanges[1] * inSplits[1]
			#ifndef SUNSHINE_TWO_CASCADES
				+ sunshine_CascadeRanges[2] * inSplits[2]
				#ifndef SUNSHINE_THREE_CASCADES
					+ sunshine_CascadeRanges[3] * inSplits[3]
				#endif
			#endif
		#endif
		;
		return cascadeRange.xy + cascadeRange.zw * shadowCoordDepth.xy;
	}
#endif

#ifdef SUNSHINE_TEXCENTERING_OFF
	#define CenterShadowTexel(coord) (coord)
#else
	#define CenterShadowTexel(coord) (floor(coord * sunshine_LightmapSize) * sunshine_LightmapTexel + sunshine_LightmapHalfTexel)
#endif

float ShadowRayLengthSquared(float3 shadowRay)
{
	float3 scaledRay = shadowRay * sunshine_ShadowToWorldScale;
	return dot(scaledRay, scaledRay);
}

//Pulse is cheaper than Random()...
#define ShadowDepthSquaredJittered(distSq) ((distSq) * (1.0 - Pulse(distSq) * sunshine_ShadowParams.y))

// ============================== SHADOW FILTERS ==============================

fixed ShadowTermHard(float3 shadowCoordDepth)
{			
	return SUNSTEP(SampleSun(shadowCoordDepth.xy), shadowCoordDepth.z);
}

fixed ShadowTermPCF2x2(float3 shadowCoordDepth)
{
	//2x2 isn't centered nicely :)
	shadowCoordDepth.xy -= sunshine_LightmapHalfTexel;
	
	//Read from the center of texels to avoid floating point error when reading neighbors...
	float2 sampleCoord = CenterShadowTexel(shadowCoordDepth.xy);
	
	float4 fSamples;
	fSamples.x = SampleSun(sampleCoord);
	fSamples.y = SampleSun(sampleCoord + sunshine_LightmapTexel * float2(1, 0));
	fSamples.z = SampleSun(sampleCoord + sunshine_LightmapTexel * float2(0, 1));
	fSamples.w = SampleSun(sampleCoord + sunshine_LightmapTexel);

	fixed4 inLight = SUNSTEP(fSamples, shadowCoordDepth.zzzz);

	fixed4 vLerps = frac(sunshine_LightmapSize * shadowCoordDepth.xy).xyxy;
	vLerps.zw = 1.0 - vLerps.zw;

	return dot(inLight, vLerps.zxzx*vLerps.wwyy);
}

fixed ShadowTermPCF3x3(float3 shadowCoordDepth)
{
	// Edge tap smoothing
	fixed4 FracWeights = frac(shadowCoordDepth.xy * sunshine_LightmapSize).xyxy;
	FracWeights.zw = 1.0-FracWeights.xy;

	//Read from the center of texels to avoid floating point error when reading neighbors...
	float2 sampleCoord = CenterShadowTexel(shadowCoordDepth.xy);

	fixed3 fShadowTerm;
	for (int y = 0; y < 3; y++)
	{
		float3 fSamples;
		fSamples.x = SampleSun(sampleCoord + sunshine_LightmapTexel * float2(-1, y - 1));
		fSamples.y = SampleSun(sampleCoord + sunshine_LightmapTexel * float2( 0, y - 1));
		fSamples.z = SampleSun(sampleCoord + sunshine_LightmapTexel * float2( 1, y - 1));

		fShadowTerm[y] = dot(SUNSTEP(fSamples, shadowCoordDepth.zzz), fixed3(FracWeights.z, 1, FracWeights.x));
	}
	
	//We multiply by 0.25 unintuitively because the frac() wittles off 1 weight from width and height, making the sum 2x2=4... so 1/4th
	return dot(fShadowTerm, fixed3(FracWeights.w, 1, FracWeights.y) * 0.25);
}
fixed ShadowTermPCF4x4(float3 shadowCoordDepth)
{
	//4x4 isn't centered nicely :)
	shadowCoordDepth.xy -= sunshine_LightmapHalfTexel;

	// Edge tap smoothing
	fixed4 FracWeights = frac(shadowCoordDepth.xy * sunshine_LightmapSize).xyxy;
	FracWeights.zw = 1.0-FracWeights.xy;

	//Read from the center of texels to avoid floating point error when reading neighbors...
	float2 sampleCoord = CenterShadowTexel(shadowCoordDepth.xy);

	fixed4 fShadowTerm = 0;
	float4 fSamples;
	
	// Unrolled loop for compatibility and cache optimization...
	
	// Top Left
	fSamples.x = SampleSun(sampleCoord + sunshine_LightmapTexel * float2(-1, -1));
	fSamples.y = SampleSun(sampleCoord + sunshine_LightmapTexel * float2(0 , -1));
	fSamples.z = SampleSun(sampleCoord + sunshine_LightmapTexel * float2(-1,  0));
	fSamples.w = SampleSun(sampleCoord);
	fixed4 inLight = SUNSTEP(fSamples, shadowCoordDepth.zzzz);
	fShadowTerm.x = dot(inLight.xy, float2(FracWeights.z, 1));
	fShadowTerm.y = dot(inLight.zw, float2(FracWeights.z, 1));

	// Top Right
	fSamples.x = SampleSun(sampleCoord + sunshine_LightmapTexel * float2(1, -1));
	fSamples.y = SampleSun(sampleCoord + sunshine_LightmapTexel * float2(2, -1));
	fSamples.z = SampleSun(sampleCoord + sunshine_LightmapTexel * float2(1,  0));
	fSamples.w = SampleSun(sampleCoord + sunshine_LightmapTexel * float2(2,  0));
	inLight = SUNSTEP(fSamples, shadowCoordDepth.zzzz);
	fShadowTerm.x += dot(inLight.xy, float2(1, FracWeights.x));
	fShadowTerm.y += dot(inLight.zw, float2(1, FracWeights.x));
	
	// Bottom Left
	fSamples.x = SampleSun(sampleCoord + sunshine_LightmapTexel * float2(-1, 1));
	fSamples.y = SampleSun(sampleCoord + sunshine_LightmapTexel * float2(0 , 1));
	fSamples.z = SampleSun(sampleCoord + sunshine_LightmapTexel * float2(-1, 2));
	fSamples.w = SampleSun(sampleCoord + sunshine_LightmapTexel * float2(0 , 2));
	inLight = SUNSTEP(fSamples, shadowCoordDepth.zzzz);
	fShadowTerm.z = dot(inLight.xy, float2(FracWeights.z, 1));
	fShadowTerm.w = dot(inLight.zw, float2(FracWeights.z, 1));
	
	// Bottom Right
	fSamples.x = SampleSun(sampleCoord + sunshine_LightmapTexel * float2(1, 1));
	fSamples.y = SampleSun(sampleCoord + sunshine_LightmapTexel * float2(2, 1));
	fSamples.z = SampleSun(sampleCoord + sunshine_LightmapTexel * float2(1, 2));
	fSamples.w = SampleSun(sampleCoord + sunshine_LightmapTexel * float2(2, 2));
	inLight = SUNSTEP(fSamples, shadowCoordDepth.zzzz);
	fShadowTerm.z += dot(inLight.xy, float2(1, FracWeights.x));
	fShadowTerm.w += dot(inLight.zw, float2(1, FracWeights.x));
	
	//We multiply by 0.11111 unintuitively because the frac() wittles off 1 weight from width and height, making the sum 3x3=9... so 1/9th
	return dot(fShadowTerm, fixed4(FracWeights.w, 1, 1, FracWeights.y) * 0.11111);
}


// ============================== SHADOW SURFACE SHADER HELPERS ==============================

// Hack Note: We can use "surfIN.sunshine_lightData" instead of "IN.cust_sunshine_lightData" if needed...
// Optimization Notes:
// 1-(depth*scale*10-9) shouldn't be in the Pixel Shader, but we want to avoid a second interpolator...
// We Pre-multiply scale*10, which gives us: 1-(depth*scaleTen-9)
// Pre-subtract (-9), which gives us 10-(depth*scaleTen)
fixed SunshineLightAttenuation(float4 lightData)
{
	float radialDistanceSq = ShadowRayLengthSquared(lightData.xyz - sunshine_ShadowCoordDepthStart.xyz);
	float2 sunCoord =
		#ifdef SUNSHINE_ONE_CASCADE
			lightData.xy;
		#else
			CalculateSunCoord(float3(lightData.xy,
			#ifdef SUNSHINE_NO_FADE
				radialDistanceSq
			#else
				ShadowDepthSquaredJittered(radialDistanceSq)
			#endif		
				));
		#endif
	float4 sunCoordDepthFade = float4(sunCoord, lightData.z,
		//sunshine_ShadowFadeParams.x-(sqrt(radialDistanceSq)*sunshine_ShadowFadeParams.y);
		//Working in r^2 Space to avoid sqrt... (See SunshineCamera.cs)
		sunshine_ShadowFadeParams.x-(radialDistanceSq*sunshine_ShadowFadeParams.y));

	#ifdef SUNSHINE_OVERCAST_OFF
		return SunshineShadowTerm(sunCoordDepthFade);
	#else
		return SunshineShadowTermOvercast(sunCoordDepthFade, OvercastTerm(lightData.xy));
	#endif
}

//Overly elaborate hack to force lightmap blending...
#ifdef LIGHTING_INCLUDED
inline UnityGI UnityGlobalIllumination_ForceMix (UnityGIInput data, half occlusion, half oneMinusRoughness, half3 normalWorld, bool reflections)
{
	UnityGI o_gi;
	UNITY_INITIALIZE_OUTPUT(UnityGI, o_gi);

	// Explicitly reset all members of UnityGI
	ResetUnityGI(o_gi);

	#if UNITY_SHOULD_SAMPLE_SH
		#if UNITY_SAMPLE_FULL_SH_PER_PIXEL
			half3 sh = ShadeSH9(half4(normalWorld, 1.0));
		#elif (SHADER_TARGET >= 30)
			half3 sh = data.ambient + ShadeSH12Order(half4(normalWorld, 1.0));
		#else
			half3 sh = data.ambient;
		#endif
	
		o_gi.indirect.diffuse += sh;
	#endif

	#if !defined(LIGHTMAP_ON)
		o_gi.light = data.light;
		o_gi.light.color *= data.atten;

	#else
		// Baked lightmaps
		fixed4 bakedColorTex = UNITY_SAMPLE_TEX2D(unity_Lightmap, data.lightmapUV.xy); 
		half3 bakedColor = DecodeLightmap(bakedColorTex);
		
		#ifdef DIRLIGHTMAP_OFF
			o_gi.indirect.diffuse = bakedColor;

			o_gi.indirect.diffuse = MixLightmapWithRealtimeAttenuation (o_gi.indirect.diffuse, data.atten, bakedColorTex);

		#elif DIRLIGHTMAP_COMBINED
			fixed4 bakedDirTex = UNITY_SAMPLE_TEX2D_SAMPLER (unity_LightmapInd, unity_Lightmap, data.lightmapUV.xy);
			o_gi.indirect.diffuse = DecodeDirectionalLightmap (bakedColor, bakedDirTex, normalWorld);

			o_gi.indirect.diffuse = MixLightmapWithRealtimeAttenuation (o_gi.indirect.diffuse, data.atten, bakedColorTex);

		#elif DIRLIGHTMAP_SEPARATE
			// Left halves of both intensity and direction lightmaps store direct light; right halves - indirect.

			// Direct
			fixed4 bakedDirTex = UNITY_SAMPLE_TEX2D_SAMPLER(unity_LightmapInd, unity_Lightmap, data.lightmapUV.xy);
			o_gi.indirect.diffuse += DecodeDirectionalSpecularLightmap (bakedColor, bakedDirTex, normalWorld, false, 0, o_gi.light);

			// Indirect
			half2 uvIndirect = data.lightmapUV.xy + half2(0.5, 0);
			bakedColor = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, uvIndirect));
			bakedDirTex = UNITY_SAMPLE_TEX2D_SAMPLER(unity_LightmapInd, unity_Lightmap, uvIndirect);
			o_gi.indirect.diffuse += DecodeDirectionalSpecularLightmap (bakedColor, bakedDirTex, normalWorld, false, 0, o_gi.light2);
		#endif
	#endif
	
	#ifdef DYNAMICLIGHTMAP_ON
		// Dynamic lightmaps
		fixed4 realtimeColorTex = UNITY_SAMPLE_TEX2D(unity_DynamicLightmap, data.lightmapUV.zw);
		half3 realtimeColor = DecodeRealtimeLightmap (realtimeColorTex);

		#ifdef DIRLIGHTMAP_OFF
			o_gi.indirect.diffuse += realtimeColor;

		#elif DIRLIGHTMAP_COMBINED
			half4 realtimeDirTex = UNITY_SAMPLE_TEX2D_SAMPLER(unity_DynamicDirectionality, unity_DynamicLightmap, data.lightmapUV.zw);
			o_gi.indirect.diffuse += DecodeDirectionalLightmap (realtimeColor, realtimeDirTex, normalWorld);

		#elif DIRLIGHTMAP_SEPARATE
			half4 realtimeDirTex = UNITY_SAMPLE_TEX2D_SAMPLER(unity_DynamicDirectionality, unity_DynamicLightmap, data.lightmapUV.zw);
			half4 realtimeNormalTex = UNITY_SAMPLE_TEX2D_SAMPLER(unity_DynamicNormal, unity_DynamicLightmap, data.lightmapUV.zw);
			o_gi.indirect.diffuse += DecodeDirectionalSpecularLightmap (realtimeColor, realtimeDirTex, normalWorld, true, realtimeNormalTex, o_gi.light3);
		#endif
	#endif
	o_gi.indirect.diffuse *= occlusion;

	if (reflections)
	{
		half3 worldNormal = reflect(-data.worldViewDir, normalWorld);

		#if UNITY_SPECCUBE_BOX_PROJECTION		
			half3 worldNormal0 = BoxProjectedCubemapDirection (worldNormal, data.worldPos, data.probePosition[0], data.boxMin[0], data.boxMax[0]);
		#else
			half3 worldNormal0 = worldNormal;
		#endif

		half3 env0 = Unity_GlossyEnvironment (UNITY_PASS_TEXCUBE(unity_SpecCube0), data.probeHDR[0], worldNormal0, 1-oneMinusRoughness);
		#if UNITY_SPECCUBE_BLENDING
			const float kBlendFactor = 0.99999;
			float blendLerp = data.boxMin[0].w;
			UNITY_BRANCH
			if (blendLerp < kBlendFactor)
			{
				#if UNITY_SPECCUBE_BOX_PROJECTION
					half3 worldNormal1 = BoxProjectedCubemapDirection (worldNormal, data.worldPos, data.probePosition[1], data.boxMin[1], data.boxMax[1]);
				#else
					half3 worldNormal1 = worldNormal;
				#endif

				half3 env1 = Unity_GlossyEnvironment (UNITY_PASS_TEXCUBE_SAMPLER(unity_SpecCube1,unity_SpecCube0), data.probeHDR[1], worldNormal1, 1-oneMinusRoughness);
				o_gi.indirect.specular = lerp(env1, env0, blendLerp);
			}
			else
			{
				o_gi.indirect.specular = env0;
			}
		#else
			o_gi.indirect.specular = env0;
		#endif
	}

	o_gi.indirect.specular *= occlusion;

	return o_gi;
}


inline void SunshineLambert_GI (
	SurfaceOutput s,
	UnityGIInput data,
	inout UnityGI gi)
{
	gi = UnityGlobalIllumination_ForceMix (data, 1.0, 0.0, s.Normal, false);
}

#endif

#define SUNSHINE_INPUT_PARAMS float4 sunshine_lightData

#define SUNSHINE_LIGHTDATA_FROM_VIEW(viewPosition) mul(sunshine_CameraVToSunVP, float4((viewPosition).xyz, 1))
#define SUNSHINE_LIGHTDATA_FROM_WORLD(worldPosition) mul(sunshine_WorldToSunVP, float4((worldPosition).xyz, 1))
#define SUNSHINE_LIGHTDATA_FROM_VERTEX(localPosition) SUNSHINE_LIGHTDATA_FROM_VIEW(mul(UNITY_MATRIX_MV, (localPosition)))

#define SUNSHINE_WRITE_VERTEX(input, output) output.sunshine_lightData = SUNSHINE_LIGHTDATA_FROM_VERTEX(input.vertex)


#if defined(UNITY_PASS_FORWARDBASE) && !defined(SUNSHINE_DISABLED) && !defined(SUNSHINE_ALPHA_DISABLED)
	#define SUNSHINE_WRITE_SURF_VERTEX(input, output) SUNSHINE_WRITE_VERTEX(input, output)
	#if !defined(SUNSHINE_DISABLED) && !defined(SUNSHINE_ALPHA_DISABLED)
		#ifdef DIRECTIONAL
			#define SUNSHINE_INJECTED
			#ifdef DIRECTIONAL_COOKIE
				#undef DIRECTIONAL_COOKIE
			#endif
			//SHADOW_ATTENUATION or LIGHT_ATTENUATION works...
			#undef SHADOW_ATTENUATION

			//IN.cust_sunshine_lightData no longer works in Unity 5
			//This should work with Unity 3, 4, and 5:
			
			#ifdef SUNSHINE_PUREPIXEL
				#define SHADOW_ATTENUATION(IN) SunshineLightAttenuation(SUNSHINE_LIGHTDATA_FROM_WORLD(worldPos))
			#else
				#define SHADOW_ATTENUATION(IN) SunshineLightAttenuation(surfIN.sunshine_lightData)
			#endif
			
			#ifdef LIGHTING_INCLUDED
			#define LightingLambert_GI SunshineLambert_GI
			#endif

		#endif
	#endif
#else
	#define SUNSHINE_WRITE_SURF_VERTEX(input, output)
#endif

#define SUNSHINE_SURFACE_VERT(input_struct) \
void sunshine_surf_vert (inout appdata_full v, out input_struct o) \
{ \
	SUNSHINE_INITIALIZE_OUTPUT(input_struct, o); \
	SUNSHINE_WRITE_SURF_VERTEX(v, o); \
}

#define SUNSHINE_SURFACE_VERT_PIGGYBACK(input_struct, piggy_vert) \
void sunshine_surf_vert (inout appdata_full v, out input_struct o) \
{ \
	SUNSHINE_INITIALIZE_OUTPUT(input_struct, o); \
	piggy_vert(v); \
	SUNSHINE_WRITE_SURF_VERTEX(v, o); \
}

#define SUNSHINE_SURFACE_VERT_PIGGYBACK2(input_struct, piggy_vert) \
void sunshine_surf_vert (inout appdata_full v, out input_struct o) \
{ \
	SUNSHINE_INITIALIZE_OUTPUT(input_struct, o); \
	piggy_vert(v, o); \
	SUNSHINE_WRITE_SURF_VERTEX(v, o); \
}

#ifdef SUNSHINE_PUREPIXEL
	#define SUNSHINE_ATTENUATION(in) SunshineLightAttenuation(SUNSHINE_LIGHTDATA_FROM_WORLD(in.worldPos))
#else
	#define SUNSHINE_ATTENUATION(in) SunshineLightAttenuation(in.sunshine_lightData)
#endif