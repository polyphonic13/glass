uniform sampler2D _MainTex;
uniform float4 _MainTex_TexelSize;

uniform sampler2D _CameraDepthTexture;
uniform sampler2D _CameraDepthNormalsTexture;

uniform float sunshine_IsOrthographic = 0.0;

uniform float3 worldLightRay;
uniform float3 worldRay;
uniform float3 worldRayU;
uniform float3 worldRayV;

float SAMPLE_DEPTH(float2 coord)
{
	float depth = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, coord));
	return sunshine_IsOrthographic ? depth : Linear01Depth(depth);
}

struct v2f {
	float4 pos : SV_POSITION;
	half2 uv : TEXCOORD0;
	half2 uv2 : TEXCOORD1;
	float4 shadowCoordDepthStart : TEXCOORD2;
	float4 shadowCoordDepthRay : TEXCOORD3;
	float3 worldRay : TEXCOORD4;
};

v2f vert( appdata_full v )
{
	v2f o;
	o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
	o.uv = v.texcoord.xy;
	o.uv2 = o.uv;
	#if UNITY_UV_STARTS_AT_TOP
		if (_MainTex_TexelSize.y < 0)
			o.uv2.y = 1.0 - o.uv2.y;
	#endif
	
	float4 rayUV = (o.uv2.x * sunshine_ShadowCoordDepthRayU) + (o.uv2.y * sunshine_ShadowCoordDepthRayV);
	o.shadowCoordDepthStart = sunshine_ShadowCoordDepthStart + rayUV * sunshine_IsOrthographic;
	o.shadowCoordDepthRay = sunshine_ShadowCoordDepthRayZ + rayUV * (1.0 - sunshine_IsOrthographic);
	
	o.worldRay = worldRay + o.uv2.x * worldRayU + o.uv2.y * worldRayV;
	
	return o;
}