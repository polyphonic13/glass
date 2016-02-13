// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

// Updated for Sunshine 1.4.5
Shader "Sunshine/Mobile/Diffuse" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 150

CGPROGRAM
#define SUNSHINE_MOBILE

#include "Assets/Sunshine/Shaders/Sunshine.cginc"
#pragma multi_compile SUNSHINE_DISABLED SUNSHINE_FILTER_PCF_2x2 SUNSHINE_FILTER_HARD

#pragma exclude_renderers flash
#pragma surface surf Lambert vertex:sunshine_surf_vert exclude_path:prepass

sampler2D _MainTex;

struct Input {
	float2 uv_MainTex;
	SUNSHINE_INPUT_PARAMS;
};
 
SUNSHINE_SURFACE_VERT(Input)

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
	o.Albedo = c.rgb;
	o.Alpha = c.a;
}
ENDCG
}

Fallback "Mobile/Diffuse"
}
