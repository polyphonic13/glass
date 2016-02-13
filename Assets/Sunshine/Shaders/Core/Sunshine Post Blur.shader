Shader "Hidden/Sunshine/PostProcess/Blur" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
}
CGINCLUDE
	#include "UnityCG.cginc"
	#include "Assets/Sunshine/Shaders/Sunshine.cginc"
	#include "Sunshine Post.cginc"

	float2 BlurXY;

ENDCG

SubShader
{
	ZTest Off
	Cull Off
	ZWrite Off
	Fog { Mode off }
		
	Pass // 0
	{
		Name "4x4 Average Blur"
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment AverageBlurFrag
			
			float BlurDepthTollerance = 0.01;
					
			float4 AverageBlurFrag(v2f input) : COLOR0
			{
				float2 xy = _MainTex_TexelSize.xy * BlurXY;
				float4 depths = float4(
					SAMPLE_DEPTH(input.uv - xy),
					SAMPLE_DEPTH(input.uv),
					SAMPLE_DEPTH(input.uv + xy),
					SAMPLE_DEPTH(input.uv + xy * 2.0));
				float4 depthsDeltas = abs(min(depths, depths.yyyy) / max(depths, depths.yyyy) - 1.0);
				float4 depthsValid = step(depthsDeltas, BlurDepthTollerance);
				float4 accum = float4(
					tex2D(_MainTex, input.uv - xy).a,
					tex2D(_MainTex, input.uv).a,
					tex2D(_MainTex, input.uv + xy).a,
					tex2D(_MainTex, input.uv + xy * 2.0).a) * depthsValid;
				return dot(accum, 1.0) / dot(depthsValid, 1.0);
			}		
		
		ENDCG
	}	
}

Fallback off

}