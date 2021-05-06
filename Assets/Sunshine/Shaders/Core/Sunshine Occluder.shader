// Upgrade NOTE: removed variant '__' where variant LOD_FADE_PERCENTAGE is used.

Shader "Hidden/Sunshine/Occluder"
{
    Properties
    {
		_MainTex ("", 2D) = "white" {}
		_Cutoff ("", Float) = 0.5
		_Color ("", Color) = (1,1,1,1)
    }
    
  	Category
	{
		Fog { Mode Off }

		/* //Dithering support needs work...
		// Shader Model 3.0 with Dithering Support
	    SubShader
		{
			Tags { "RenderType" = "Opaque" }
			Pass
		    {
				CGPROGRAM
					#include "Sunshine Occluder Common.cginc"
					#pragma target 3.0
					#pragma multi_compile _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
					#pragma vertex vert
					#pragma fragment opaqueFrag
				ENDCG
		    }
		}
		// Shader Model 2.0 without Dithering Support
		*/
	    SubShader
		{
			Tags { "RenderType" = "Opaque" }
			Pass
		    {
				CGPROGRAM
					#include "Sunshine Occluder Common.cginc"
					#pragma multi_compile _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON
					#pragma vertex vert
					#pragma fragment opaqueFrag
				ENDCG
		    }
		}
		SubShader
		{
			Tags { "RenderType" = "TransparentCutout" }
			Pass
		    {
		    	//Cull Off
				CGPROGRAM
					#include "Sunshine Occluder Common.cginc"
					#pragma vertex vert
					#pragma fragment constColorCutoutFrag
				ENDCG
		    }
		}
		SubShader
		{
			Tags { "RenderType" = "Transparent" }
			Pass
		    {
		    	//Cull Off
				CGPROGRAM
					#include "Sunshine Occluder Common.cginc"
					#pragma vertex vert
					#pragma fragment alphaFrag
				ENDCG
		    }
		}
	    SubShader
		{
			Tags { "RenderType" = "TreeOpaque" "DisableBatching"="True" }
			Pass
		    {
				CGPROGRAM
					#include "Sunshine Occluder Common.cginc"
					#pragma vertex treeVert
					#pragma fragment opaqueFrag
				ENDCG
		    }
		}
		SubShader
		{
			Tags { "RenderType" = "TreeTransparentCutout" "DisableBatching"="True" }
			Pass
		    {
		    	Cull Off
				CGPROGRAM
					#include "Sunshine Occluder Common.cginc"
					#pragma vertex treeVert
					#pragma fragment cutoutFrag
				ENDCG
		    }
		}

		SubShader
		{
			Tags { "RenderType" = "TreeBark" }
			Pass
			{
				CGPROGRAM
					#include "Sunshine Occluder Common.cginc"
					#pragma vertex barkVert
					#pragma fragment opaqueFrag
					#pragma glsl_no_auto_normalization
					v2f barkVert( appdata_full v )
					{
	  				 	TreeVertBark(v);
	  				 	return vert(v);
					}
				ENDCG
			}
		}
		SubShader
		{
			Tags { "RenderType" = "TreeLeaf" }
			Pass
			{
				//Cull Off
				CGPROGRAM
					#include "Sunshine Occluder Common.cginc"
					#pragma vertex leafVert
					#pragma fragment cutoutFrag
					#pragma glsl_no_auto_normalization
					v2f leafVert( appdata_full v )
					{
	  				 	TreeVertLeaf(v);
	  				 	return vert(v);
					}
				ENDCG
			}
		}
		SubShader
		{
			Tags { "RenderType" = "Grass" }
			Pass
		    {
		    	Cull Off
				CGPROGRAM
					#include "Sunshine Occluder Common.cginc"
					#pragma vertex grassVert
					#pragma fragment vertColorCutoutFrag
					v2f grassVert( appdata_full v )
					{
						v2f o;
					    SUNSHINE_INITIALIZE_OUTPUT(v2f, o);
					 	WavingGrassVert(v);
						o.color = v.color;
					 	o.uv = v.texcoord.xy;
					 	return baseVert(v, o);
					}
				ENDCG
		    }
		}
		/*
		// Not ready for primetime...
		SubShader
		{
			Tags { "RenderType" = "GrassBillboard" }
			Pass
		    {
		    	Cull Off
				CGPROGRAM
					#include "Sunshine Occluder Common.cginc"
					#pragma vertex grassVert
					#pragma fragment vertColorCutoutFrag
					v2f grassVert( appdata_full v )
					{
						v2f o;
					    SUNSHINE_INITIALIZE_OUTPUT(v2f, o);
					 	WavingGrassBillboardVert(v);
						o.color = v.color;
					 	o.uv = v.texcoord.xy;
					 	return baseVertNoSlope(v, o);
					}
				ENDCG
		    }
		}
		*/
		SubShader
		{
			Cull [_Cull]
			Tags {
			"RenderType" = "SpeedTree"
			"DisableBatching"="LODFading"
			}
			Pass
			{
				CGPROGRAM
				#pragma vertex speedtree_vert
				#pragma fragment speedtree_frag
				#pragma target 3.0
				#pragma multi_compile  LOD_FADE_PERCENTAGE
				#pragma multi_compile GEOM_TYPE_BRANCH GEOM_TYPE_BRANCH_DETAIL GEOM_TYPE_BRANCH_BLEND GEOM_TYPE_FROND GEOM_TYPE_LEAF GEOM_TYPE_FACING_LEAF GEOM_TYPE_MESH
				#define ENABLE_WIND
				#define _MainTex _MainTex_Dummy
				#define _Color _Color_Dummy
				#define _Cutoff _Cutoff_Dummy
				#define TriangleWave TriangleWaveSpeedTree
				#include "SpeedTreeCommon.cginc"
				#undef TriangleWave
				#undef _Cutoff
				#undef _Color
				#undef _MainTex
				
				#define base_appdata SpeedTreeVB
				#include "Sunshine Occluder Common.cginc"

				v2f speedtree_vert(SpeedTreeVB v)
				{
					v2f o;
				    SUNSHINE_INITIALIZE_OUTPUT(v2f, o);
					#ifdef SPEEDTREE_ALPHATEST
						o.uv = v.texcoord.xy;
					#endif
					OffsetSpeedTreeVertex(v, unity_LODFade.x);
					return baseVert(v, o);
				}

				float4 speedtree_frag(v2f i) : SV_Target
				{
					#ifdef SPEEDTREE_ALPHATEST
						clip(tex2D(_MainTex, i.uv).a * _Color.a - _Cutoff);
					#endif
					return frag(i);
				}
				
				ENDCG
			}
		}
		/*
		SubShader
		{
			Cull Off
			Tags { "RenderType" = "AtsFoliage" }
			Pass
			{
					CGPROGRAM
						#include "Sunshine Occluder Common.cginc"
						#define _Color _AtsColor
						#include "../../../Advanced Foliage Shader v2.041/Shaders/Includes/Tree.cginc"
						#include "../../../Advanced Foliage Shader v2.041/Shaders/Includes/CustomBending.cginc"
						#undef _Color
						#undef SLOPE_BIAS
						#pragma vertex vertAtsFoliage
						#pragma fragment cutoutFrag
						
						v2f vertAtsFoliage( appdata_full v )
						{
							v2f o;
						    SUNSHINE_INITIALIZE_OUTPUT(v2f, o);
							CustomBending (v);
							o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
						 	return baseVert(v, o);
						}
						
					ENDCG

			}
		}
		SubShader
		{
			Cull Off
			Tags { "RenderType" = "AtsFoliageTouchBending" }
			Pass
			{
					CGPROGRAM
						#include "Sunshine Occluder Common.cginc"
						#define _Color _AtsColor
						#include "../../../Advanced Foliage Shader v2.041/Shaders/Includes/Tree.cginc"
						#include "../../../Advanced Foliage Shader v2.041/Shaders/Includes/TouchBending.cginc"
						#undef _Color
						#undef SLOPE_BIAS
						#pragma vertex vertAtsFoliage
						#pragma fragment cutoutFrag
						
						v2f vertAtsFoliage( appdata_full v )
						{
							v2f o;
						    SUNSHINE_INITIALIZE_OUTPUT(v2f, o);
							TouchBending (v);
							o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
						 	return baseVert(v, o);
						}
						
					ENDCG

			}
		}
		*/
	}
	Fallback Off
}