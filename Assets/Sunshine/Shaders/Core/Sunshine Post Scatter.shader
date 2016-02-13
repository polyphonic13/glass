// Updated for Sunshine 1.4.5
Shader "Hidden/Sunshine/PostProcess/Scatter" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_ScatterRamp ("Scatter Ramp (RGB)", 2D) = "white" {}
}
CGINCLUDE

	//Low Quality:
	#if defined(SUNSHINE_FILTER_HARD)
		#define RAY_PASSES 2
	//Medium Quality:
	#elif defined(SUNSHINE_FILTER_PCF_2x2)
		#define RAY_PASSES 3
	//High Quality:
	#elif defined(SUNSHINE_FILTER_PCF_3x3)
		#define RAY_PASSES 4
	//Very High Quality:
	#elif defined(SUNSHINE_FILTER_PCF_4x4)
		#define RAY_PASSES 5
	//Mobile Quality:
	#else
		#define RAY_PASSES 1
	#endif		

	#include "UnityCG.cginc"
	#include "Assets/Sunshine/Shaders/Sunshine.cginc"
	#include "Sunshine Post.cginc"

	uniform sampler2D _ScatterTexture;

	float4 ScatterColor = 1.0;
	float4 ScatterIntensityVolumeSky;
	float ScatterVolumeScale = 1.0;
	
	float3 ScatterDitherData;
	sampler2D ScatterDitherMap;
	sampler2D _ScatterRamp;

	
	#define RAY_SAMPLES (RAY_PASSES * 4)
	#define RAY_STEP (1.0 / RAY_SAMPLES)

	// Ray-marches through to 4 Lightmap cascades and estimates the volumetric light:
	float4 scatterFrag (v2f input) : COLOR
	{	
		// Get the screen-space normalized depth:
		float depth01 = SAMPLE_DEPTH( input.uv2);
		
		// If the depth is close to 1.0, count it as the "Sky":
		bool isSky = (depth01 > 0.99);
		
		// Clamp the depth to the shadow's FarClip, since we can only scan within the Lightmap's visible range:
		depth01 = min(sunshine_ShadowFadeParams.z/sqrt(ShadowRayLengthSquared(input.shadowCoordDepthRay)), depth01);
		
		// Grab the texture coordinates, and shadow-space depth:
		float4 scaledCoordDepthRay = input.shadowCoordDepthRay * depth01;
		
		float scaledRayLengthSq = ShadowRayLengthSquared(scaledCoordDepthRay);
		
		// Debug Depth Range
		//return step(sqrt(ShadowRayLengthSquared(scaledCoordDepthRay)) / sunshine_ShadowFadeParams.z, 0.999);

		// Generate a random value to jitter the sampling pattern:
		//float fRandom = Random(input.uv + ScatterDitherData.zz);
		float fRandom = frac(tex2D(ScatterDitherMap, input.uv * ScatterDitherData.xy).a + ScatterDitherData.z);

		// Sample accumulator
		float accum = 0;
		
		for(int i = 0; i < RAY_PASSES; i++)
		{
			// Calculate 4 normalized sample locations along the eye ray:
			float4 ratios = (float4(0, 1, 2, 3) + (i * 4) + fRandom) * RAY_STEP;
		
			// Calculate 4 sample depths in shadow space:
			float4 iLightDepths = input.shadowCoordDepthStart.zzzz + scaledCoordDepthRay.zzzz * ratios;

			// Calculate 4 sample depths in squared camera space:
			float4 iEyeDepths = ratios*ratios * scaledRayLengthSq;

		 	// Debug Cascades:
		 	//if(i==RAY_PASSES-1){ float4 cdbg=SunshineInCascades(iEyeDepths.w); return float4(cdbg.rgb+float3(cdbg.a,0,cdbg.a), 1); }

			// Calculate the first 2 samples' Lightmap texture coordinates in first cascade:
			float4 iTexCoords = input.shadowCoordDepthStart.xyxy + scaledCoordDepthRay.xyxy * ratios.xxyy;
			
			#ifdef SUNSHINE_OVERCAST_ON
				// Get the first 2 Overcast Shadow Terms:
				float4 overcastTerms = float4(OvercastTerm(iTexCoords.xy), OvercastTerm(iTexCoords.zw), 0, 0);
			#endif

			// Read the first 2 Lightmap samples:
			float4 samples =	SampleSun(CalculateSunCoord(float3(iTexCoords.xy, iEyeDepths.x)));
			samples.y =			SampleSun(CalculateSunCoord(float3(iTexCoords.zw, iEyeDepths.y)));
			
			// Calculate the second 2 samples' Lightmap texture coordinates in first cascade:
			iTexCoords = input.shadowCoordDepthStart.xyxy + scaledCoordDepthRay.xyxy * ratios.zzww;
			
			#ifdef SUNSHINE_OVERCAST_ON
				// Get the second 2 Overcast Shadow Terms:
				overcastTerms.zw = float2(OvercastTerm(iTexCoords.xy), OvercastTerm(iTexCoords.zw));
			#endif

			// Read the second 2 Lightmap samples:
			samples.z =			SampleSun(CalculateSunCoord(float3(iTexCoords.xy, iEyeDepths.z)));
			samples.w =			SampleSun(CalculateSunCoord(float3(iTexCoords.zw, iEyeDepths.w)));
			
			// Check which of the 4 samples are in light:
			float4 inLight = step(iLightDepths, samples);
			
			#ifdef SUNSHINE_OVERCAST_ON
				// Apply Overcast Terms:
				inLight *= overcastTerms;
			#endif
		
			// Accumulate the inLight values, weighted appropriately:
			accum += dot(inLight, RAY_STEP);
		}
		
		// Apply Scatter Ramp texture...
		float dir = dot(normalize(input.worldRay), worldLightRay) * 0.5 + 0.5;
		accum *= tex2D(_ScatterRamp, float2(dir, 0.5)).r;
		
		// Calculate the final color, in a way that "looks nice" in practice:
		float scatter01 = accum * min(1.0, depth01 * ScatterIntensityVolumeSky.y);
		
		// Adjust the value if this texel represents the sky:
		if(isSky)
			scatter01 = scatter01 * ScatterIntensityVolumeSky.z + ScatterIntensityVolumeSky.w;
		
		// Blend the scatter color into the scene:
		return float4(lerp(tex2D(_MainTex, input.uv), ScatterColor, scatter01 * ScatterIntensityVolumeSky.x).rgb, scatter01);
	}
	float4 applyScatterFrag (v2f input) : COLOR
	{
		// Blend the scatter color into the scene:
		float4 scatter = tex2D(_ScatterTexture, input.uv2);
		return lerp(tex2D(_MainTex, input.uv), ScatterColor, scatter.a * ScatterIntensityVolumeSky.x);
	}		
ENDCG

// Desktop Version
SubShader
{
	ZTest Off
	Cull Off
	ZWrite Off
	Fog { Mode off }
		
	Pass // 0
	{
    	Name "Light Scattering (Desktop)"
		CGPROGRAM
			#pragma target 3.0
			#pragma exclude_renderers gles

			// To save Shader Keywords, we'll recycle Filter Keywords:
			#pragma multi_compile SUNSHINE_FILTER_HARD SUNSHINE_FILTER_PCF_2x2 SUNSHINE_FILTER_PCF_3x3 SUNSHINE_FILTER_PCF_4x4
			//#pragma multi_compile SUNSHINE_SCATTER_QUALITY_LOW SUNSHINE_SCATTER_QUALITY_MEDIUM SUNSHINE_SCATTER_QUALITY_HIGH SUNSHINE_SCATTER_QUALITY_VERYHIGH

			
			#pragma multi_compile SUNSHINE_OVERCAST_ON SUNSHINE_OVERCAST_OFF
			
			// Optimize for various cascade situations:
			#pragma multi_compile SUNSHINE_ONE_CASCADE SUNSHINE_TWO_CASCADES SUNSHINE_FOUR_CASCADES

			#pragma vertex vert
			#pragma fragment scatterFrag
			
			
		ENDCG
	}
	Pass // 1
	{
		Name "Apply Scatter"
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment applyScatterFrag
		ENDCG
	}
}

// Mobile Version
SubShader
{
	ZTest Off
	Cull Off
	ZWrite Off
	Fog { Mode off }
		
	Pass // 0
	{
    	Name "Light Scattering (Mobile)"
		CGPROGRAM
		 	#pragma exclude_renderers flash

			#pragma multi_compile SUNSHINE_OVERCAST_OFF
			
			// Mobile only allows One Cascade!:
			#pragma multi_compile SUNSHINE_ONE_CASCADE

			#pragma vertex vert
			#pragma fragment scatterFrag
			
			
		ENDCG
	}
	Pass // 1
	{
		Name "Apply Scatter"
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment applyScatterFrag
		ENDCG
	}
}
Fallback off

}