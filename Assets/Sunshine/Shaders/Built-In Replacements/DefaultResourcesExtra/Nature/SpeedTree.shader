// Upgrade NOTE: removed variant '__' where variant LOD_FADE_PERCENTAGE is used.

Shader "Sunshine/Nature/SpeedTree"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_SpecColor ("Specular Color", Color) = (0,0,0,0)
		_HueVariation ("Hue Variation", Color) = (1.0,0.5,0.0,0.1)
		_Shininess ("Shininess", Range (0.01, 1)) = 0.1
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_DetailTex ("Detail", 2D) = "black" {}
		_BumpMap ("Normal Map", 2D) = "bump" {}
		_Cutoff ("Alpha Cutoff", Range(0,1)) = 0.333
		[MaterialEnum(Off,0,Front,1,Back,2)] _Cull ("Cull", Int) = 2
		[MaterialEnum(None,0,Fastest,1,Fast,2,Better,3,Best,4,Palm,5)] _WindQuality ("Wind Quality", Range(0,5)) = 0
	}

	// targeting SM3.0+
	SubShader
	{
		Tags
		{
			"Queue"="Geometry"
			"IgnoreProjector"="True"
			"RenderType"="SpeedTree"
			"DisableBatching"="LODFading"
		}
		LOD 400
		Cull [_Cull]

		CGPROGRAM
		
			#define SUNSHINE_PUREPIXEL
			#include "Assets/Sunshine/Shaders/Sunshine.cginc"
			#pragma multi_compile SUNSHINE_DISABLED SUNSHINE_FILTER_PCF_4x4 SUNSHINE_FILTER_PCF_3x3 SUNSHINE_FILTER_PCF_2x2 SUNSHINE_FILTER_HARD
		
			#pragma surface surf Lambert vertex:SpeedTreeVert nolightmap
			#pragma target 3.0
			#pragma multi_compile  LOD_FADE_PERCENTAGE LOD_FADE_CROSSFADE
			#pragma shader_feature GEOM_TYPE_BRANCH GEOM_TYPE_BRANCH_DETAIL GEOM_TYPE_BRANCH_BLEND GEOM_TYPE_FROND GEOM_TYPE_LEAF GEOM_TYPE_FACING_LEAF GEOM_TYPE_MESH
			#pragma shader_feature EFFECT_BUMP
			#pragma shader_feature EFFECT_HUE_VARIATION
			#define ENABLE_WIND
			#include "SpeedTreeCommon.cginc"

			void surf(Input IN, inout SurfaceOutput OUT)
			{
				SpeedTreeFragOut o;
				SpeedTreeFrag(IN, o);
				SPEEDTREE_COPY_FRAG(OUT, o)
			}
		ENDCG

	}

	FallBack "Nature/SpeedTree"
	CustomEditor "SpeedTreeMaterialInspector"
}
