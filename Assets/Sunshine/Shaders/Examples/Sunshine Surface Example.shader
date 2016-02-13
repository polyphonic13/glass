Shader "Sunshine/Examples/Surface Shader Example" { // Prefix the name with "Sunshine/"
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 200

CGPROGRAM
// Include Sunshine functionality:
#include "Assets/Sunshine/Shaders/Sunshine.cginc"

// Multi-compile for different filters:
#pragma multi_compile SUNSHINE_DISABLED SUNSHINE_FILTER_PCF_4x4 SUNSHINE_FILTER_PCF_3x3 SUNSHINE_FILTER_PCF_2x2 SUNSHINE_FILTER_HARD

// Require SM 3.0 on Desktop platforms:
#pragma target 3.0

// Use the sunshine_surf_vert modifier, and exclude prepass/forwardadd to cut waste.
#pragma surface surf Lambert vertex:sunshine_surf_vert exclude_path:prepass

sampler2D _MainTex;
fixed4 _Color;

struct Input {
	float2 uv_MainTex;
	// Required Sunshine Params:
	SUNSHINE_INPUT_PARAMS;
};
 
// Generates sunshine_surf_vert modifier:
SUNSHINE_SURFACE_VERT(Input)

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	o.Albedo = c.rgb;
	o.Alpha = c.a;
}
ENDCG
}

// Fallback to original shader:
Fallback "Diffuse"
}
