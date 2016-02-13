Shader "Hidden/Sunshine/TerrainEngine/Details/WavingDoublePass" {
Properties {
	_WavingTint ("Fade Color", Color) = (.7,.6,.5, 0)
	_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
	_WaveAndDistance ("Wave and distance", Vector) = (12, 3.6, 1, 1)
	_Cutoff ("Cutoff", float) = 0.5
}

SubShader {
	Tags {
		"Queue" = "Geometry+200"
		"IgnoreProjector"="True"
		"RenderType"="Grass"
		"DisableBatching"="True"
	}
	Cull Off
	LOD 200
		
CGPROGRAM
//#pragma surface surf Lambert vertex:WavingGrassVert addshadow
#include "TerrainEngine.cginc"

#include "Assets/Sunshine/Shaders/Sunshine.cginc"
//Grass doesn't require higher quality filters:
#pragma multi_compile SUNSHINE_DISABLED SUNSHINE_FILTER_PCF_2x2 SUNSHINE_FILTER_HARD
#pragma target 3.0
#pragma surface surf Lambert vertex:sunshine_surf_vert exclude_path:prepass nolightmap


sampler2D _MainTex;
fixed _Cutoff;

struct Input {
	float2 uv_MainTex;
	fixed4 color : COLOR;
	SUNSHINE_INPUT_PARAMS;
};

SUNSHINE_SURFACE_VERT_PIGGYBACK(Input, WavingGrassVert)

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
	o.Albedo = c.rgb;
	o.Alpha = c.a;
	clip (o.Alpha - _Cutoff);
	o.Alpha *= IN.color.a;
}
ENDCG
}
	
	
	
	Fallback "Hidden/TerrainEngine/Details/WavingDoublePass"
}
