Shader "Hidden/Sunshine/PostProcess/Debug" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
}
CGINCLUDE
	#include "UnityCG.cginc"
	#include "Assets/Sunshine/Shaders/Sunshine.cginc"
	#include "Sunshine Post.cginc"
ENDCG

SubShader
{
	ZTest Always Cull Off ZWrite Off
	Fog { Mode off }
		
	Pass // 0
	{
    	Name "Debug Cascades"
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			fixed4 frag (v2f input) : COLOR
			{
				float depth01 = SAMPLE_DEPTH(input.uv2);
				
				float radialDistanceSq = ShadowRayLengthSquared(input.shadowCoordDepthRay.xyz * depth01);
				
				float4 inSplits = SunshineInCascades(radialDistanceSq);
				float4 splitColor =
					float4(1,0,0,0)*inSplits[0] + 
					float4(0,1,0,0)*inSplits[1] + 
					float4(0,0,1,0)*inSplits[2] + 
					float4(1,0,1,0)*inSplits[3];
				
				return lerp(tex2D(_MainTex, input.uv), splitColor, 0.25);
			}
		ENDCG
	}	
	Pass // 1
	{
    	Name "Debug Alpha"
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			fixed4 frag (v2f input) : COLOR
			{
				return fixed4(tex2D(_MainTex, input.uv).aaa, 1.0);
			}
		ENDCG
	}	
	Pass // 2
	{
    	Name "Debug Lightmap"
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			fixed4 frag (v2f input) : COLOR
			{
				return SampleSun(input.uv);
			}
		ENDCG
	}	
}

Fallback off

}