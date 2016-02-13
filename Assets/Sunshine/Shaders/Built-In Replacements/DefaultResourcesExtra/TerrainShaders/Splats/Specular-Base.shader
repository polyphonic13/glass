Shader "Hidden/TerrainEngine/Splatmap/Specular-Base" {
	Properties {
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess ("Shininess", Range (0.03, 1)) = 0.078125
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}

		// used in fallback on old cards
		_Color ("Main Color", Color) = (1,1,1,1)
	}

	SubShader { 
		Tags {
			"RenderType" = "Opaque"
			"Queue" = "Geometry-100"
		}
		LOD 200

		CGPROGRAM
		#include "Assets/Sunshine/Shaders/Sunshine.cginc"
		#pragma multi_compile SUNSHINE_DISABLED SUNSHINE_FILTER_PCF_4x4 SUNSHINE_FILTER_PCF_3x3 SUNSHINE_FILTER_PCF_2x2 SUNSHINE_FILTER_HARD
		#pragma target 3.0

		#pragma surface surf BlinnPhong vertex:sunshine_surf_vert exclude_path:prepass exclude_path:deferred
		
		sampler2D _MainTex;
		half _Shininess;

		struct Input {
			float2 uv_MainTex;
			SUNSHINE_INPUT_PARAMS;
		};

		SUNSHINE_SURFACE_VERT(Input)
		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = tex.rgb;
			o.Gloss = tex.a;
			o.Alpha = 1.0f;
			o.Specular = _Shininess;
		}
		ENDCG
	}

	FallBack "Sunshine/Legacy Shaders/Specular"
}
