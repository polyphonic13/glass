// == Standard Shadow Stuff ==
//#define _Color _DummyStandardColor
//#include "UnityStandardShadow.cginc"
//#undef _Color
sampler2D _MainTex;
float4 _MainTex_ST;
fixed _Cutoff;
// == Standard Shadow Stuff ==


#include "UnityCG.cginc"
#include "Lighting.cginc"

#include "UnityBuiltin3xTreeLibrary.cginc"

#define SHADOW_CUTOFF(alpha, cutoff) clip((alpha) - (cutoff))

#include "Assets/Sunshine/Shaders/Sunshine.cginc"
#define SLOPE_BIAS

float2 sunshine_DepthBiases;
#define sunshine_DepthBias (sunshine_DepthBiases.x)
#define sunshine_DepthSlopeBias (sunshine_DepthBiases.y)
					
struct v2f {
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
	fixed4 color : TEXCOORD1;
	float2 depthAndBias : TEXCOORD2;
	//#ifdef UNITY_STANDARD_USE_DITHER_MASK
	//UNITY_VPOS_TYPE vpos : VPOS;
	//#endif
};


#ifdef SLOPE_BIAS
#define BASEVERT_DOSLOPE o.depthAndBias.y = sunshine_DepthBias + ((1.0 - abs(normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal.xyz)).z)) * sunshine_DepthSlopeBias);
#else
	#define BASEVERT_DOSLOPE
#endif

#ifndef base_appdata
#define base_appdata appdata_full
#endif
void baseVert_Depth (base_appdata v, inout v2f o)
{
    o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
    o.depthAndBias = mul(UNITY_MATRIX_MV, v.vertex).z;
}
v2f baseVert (base_appdata v, v2f o)
{
	baseVert_Depth(v, o);
	BASEVERT_DOSLOPE
    return o;
}

v2f baseVertNoSlope (base_appdata v, v2f o)
{
	baseVert_Depth(v, o);
    return o;
}
v2f vert (base_appdata v) 
{
    v2f o;
    SUNSHINE_INITIALIZE_OUTPUT(v2f, o);
    o.uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
    return baseVert(v, o);
}

v2f treeVert( base_appdata v )
{
	v2f o;
    SUNSHINE_INITIALIZE_OUTPUT(v2f, o);
 	TerrainAnimateTree(v.vertex, v.color.w);
 	o.uv = v.texcoord.xy;
 	return baseVert(v, o);
}

fixed4 frag (v2f i)
{
    #ifdef SLOPE_BIAS
		float depth = (-i.depthAndBias.x + max(0.0, i.depthAndBias.y)) * _ProjectionParams.w;
    #else
		float depth = (-i.depthAndBias.x + sunshine_DepthBias) * _ProjectionParams.w;
	#endif
	return WriteSun(min(depth, 0.995));
}

fixed4 cutoutFrag (v2f i) : COLOR
{
	half alpha = SUNSHINE_SAMPLE_1CHANNEL(_MainTex, i.uv);
//#ifdef UNITY_STANDARD_USE_DITHER_MASK
//	SHADOW_CUTOFF(tex3D(_DitherMaskLOD, float3(i.vpos.xy*0.25,alpha*0.9375)).a, _Cutoff);
//#else
	SHADOW_CUTOFF(alpha, _Cutoff);
//#endif
	return frag(i);
}

fixed4 opaqueFrag (v2f i) : COLOR
{
#if defined(_ALPHATEST_ON) || defined(_ALPHABLEND_ON) || defined(_ALPHAPREMULTIPLY_ON)
	return cutoutFrag(i);
#else
	return frag(i);
#endif
}


fixed4 alphaFrag (v2f i) : COLOR
{
	SHADOW_CUTOFF(SUNSHINE_SAMPLE_1CHANNEL(_MainTex, i.uv) * _Color.a, 0.75);
	return frag(i);
}
fixed4 constColorCutoutFrag (v2f i) : COLOR
{
	SHADOW_CUTOFF(SUNSHINE_SAMPLE_1CHANNEL(_MainTex, i.uv) * _Color.a, _Cutoff);
	return frag(i);
}
fixed4 vertColorCutoutFrag (v2f i) : COLOR
{
	SHADOW_CUTOFF(SUNSHINE_SAMPLE_1CHANNEL(_MainTex, i.uv) * i.color.a, _Cutoff);
	return frag(i);
}