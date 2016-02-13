Shader "Hidden/Sunshine TerrainEngine/Details/BillboardWavingDoublePass" {
	Properties {
		_WavingTint ("Fade Color", Color) = (.7,.6,.5, 0)
		_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
		_WaveAndDistance ("Wave and distance", Vector) = (12, 3.6, 1, 1)
		_Cutoff ("Cutoff", float) = 0.5
	}
	
CGINCLUDE
#include "UnityCG.cginc"
#include "TerrainEngine.cginc"

struct v2f {
	float4 pos : SV_POSITION;
	fixed4 color : COLOR;
	float4 uv : TEXCOORD0;
};
v2f BillboardVert (appdata_full v) {
	v2f o;
	WavingGrassBillboardVert (v);
	o.color = v.color;
	
	o.color.rgb *= ShadeVertexLights (v.vertex, v.normal);
		
	o.pos = mul (UNITY_MATRIX_MVP, v.vertex);	
	o.uv = v.texcoord;
	return o;
}
ENDCG

	SubShader {
		Tags {
			"Queue" = "Geometry+200"
			"IgnoreProjector"="True"
			"RenderType"="GrassBillboard"
			"DisableBatching"="True"
		}
		Cull Off
		LOD 200
				
CGPROGRAM
//#pragma surface surf Lambert vertex:WavingGrassBillboardVert addshadow

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

SUNSHINE_SURFACE_VERT_PIGGYBACK(Input, WavingGrassBillboardVert)

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * IN.color;
	o.Albedo = c.rgb;
	o.Alpha = c.a;
	clip (o.Alpha - _Cutoff);
	o.Alpha *= IN.color.a;
}

ENDCG			
	}

	Fallback "Hidden/TerrainEngine/Details/BillboardWavingDoublePass"
}
