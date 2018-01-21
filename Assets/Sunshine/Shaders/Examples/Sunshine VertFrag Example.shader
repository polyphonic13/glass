// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sunshine/Examples/VertFrag Example" {
	SubShader {
		Tags { "RenderType" = "Opaque" }
		Pass {
			Tags { "LightMode" = "ForwardBase" }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			#include "Assets/Sunshine/Shaders/Sunshine.cginc"
			#pragma multi_compile SUNSHINE_DISABLED SUNSHINE_FILTER_PCF_4x4 SUNSHINE_FILTER_PCF_3x3 SUNSHINE_FILTER_PCF_2x2 SUNSHINE_FILTER_HARD

			// Require SM 3.0 on Desktop platforms:
			#pragma target 3.0
			

			struct vertOut {
				float4 pos:SV_POSITION;
				
				// Required Sunshine Params:
				// Specify a Semantic for DX11:
				SUNSHINE_INPUT_PARAMS : TEXCOORD0;
			};

			vertOut vert(appdata_base v) {
				vertOut o;
				o.pos = UnityObjectToClipPos (v.vertex);
				
				//Write Sunshine Params:
				SUNSHINE_WRITE_VERTEX(v, o);
				return o;
			}

			fixed4 frag(vertOut i) : COLOR0
			{
				//Get Sunshine Light Attenuation:
				fixed lightAttenuation = SUNSHINE_ATTENUATION(i);
				
				//This is a simple example, just return the attenuation :)
				return lightAttenuation;
			}

			ENDCG
		}
	}
	Fallback "Diffuse"
}