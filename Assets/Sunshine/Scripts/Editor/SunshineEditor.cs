using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Sunshine))]
public class SunshineEditor : Editor
{
	private const string _ShowAdvancedPref = "Sunshine_ShowAdvanced";
	private static bool ShowAdvanced
	{
		get { return EditorPrefs.GetBool(_ShowAdvancedPref, false); }
		set { EditorPrefs.SetBool(_ShowAdvancedPref, value); }
	}
	private const string _ShowForwardShadowsPref = "Sunshine_ShowForwardShadows";
	private static bool ShowForwardShaders
	{
		get { return EditorPrefs.GetBool(_ShowForwardShadowsPref, true); }
		set { EditorPrefs.SetBool(_ShowForwardShadowsPref, value); }
	}
	private const string _DisplayLightmapPref = "Sunshine_DisplayLightmap";
	private static bool DisplayLightmap
	{
		get { return EditorPrefs.GetBool(_DisplayLightmapPref, false); }
		set { EditorPrefs.SetBool(_DisplayLightmapPref, value); }
	}

	private Sunshine sun { get { return target as Sunshine; } }
	
	SerializedProperty Prop(string name)
	{
		return serializedObject.FindProperty(name);
	}
	
	void OvercastSettings(string prefix)
	{
		var propOvercastTexture = Prop(prefix + "Texture");
		var propOvercastScale = Prop(prefix + "Scale");
		var propOvercastMovement = Prop(prefix + "Movement");
		var propOvercastPlaneHeight = Prop(prefix + "PlaneHeight");
		
		SunshineEditorHelper.Horizontal(()=>
		{
			propOvercastTexture.objectReferenceValue = EditorGUILayout.ObjectField("Overcast Cookie", propOvercastTexture.objectReferenceValue, typeof(Texture2D), true);
			SunshineEditorHelper.EnableGroup(propOvercastTexture.objectReferenceValue, ()=>
			{
				SunshineEditorHelper.Vertical(()=>
				{
					float oldLabelWidth = EditorGUIUtility.labelWidth;
					EditorGUIUtility.labelWidth = 80;
					propOvercastScale.floatValue = EditorGUILayout.FloatField(new GUIContent("Scale", "Physical size of the Ovecast Cookie"), propOvercastScale.floatValue);
					propOvercastPlaneHeight.floatValue = EditorGUILayout.FloatField(new GUIContent("Plane Height", "Virtual height of the Overcast Plane"), propOvercastPlaneHeight.floatValue);
					propOvercastMovement.vector2Value = EditorGUILayout.Vector2Field("Movement", propOvercastMovement.vector2Value);
					EditorGUIUtility.labelWidth = oldLabelWidth;
				});
			});
		});
		EditorGUILayout.Space();
	}
	
	string CascadeCountString(int cascadeCount)
	{
		string result = "No Cascades";
		switch(cascadeCount)
		{
		case 2: result = "Two Cascades"; break;
		case 3: result = "Three Cascades"; break;
		case 4: result = "Four Cascades"; break;
		}
		if(cascadeCount != sun.CustomCascadeCount)
		{
			if(sun.IsMobile)
				result += " (on mobile)";
		}
		return result;
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update();
		
		SunshineEditorHelper.ClearColor(); //Just to be safe;
		SunshineEditorHelper.PushColor(Color.Lerp(Color.white, Color.yellow, 0.15f));

		var propOccluderShader = Prop("OccluderShader");
		var propPostScatterShader = Prop("PostScatterShader");
		var propPostBlurShader = Prop("PostBlurShader");
		var propPostDebugShader = Prop("PostDebugShader");
		var propBlankOvercastTexture = Prop("BlankOvercastTexture");

		var propShaderSet = Prop("ShaderSet");
		var propSunLight = Prop("SunLight");
		
		var propOccluders = new SerializedProperty[]
		{
			Prop("Occluders"),
			Prop("Occluders1"),
			Prop("Occluders2"),
			Prop("Occluders3")
		};
		var propUsePerCascadeOccluders = Prop("UsePerCascadeOccluders");
		
		var propResolution = Prop("LightmapResolution");
		var propCustomResolution = Prop("CustomLightmapResolution");
		var propShadowFilter = Prop("ShadowFilter");
		
		var propUseLODFix = Prop("UseLODFix");
		var propUseOcclusionCulling = Prop("UseOcclusionCulling");

		var propUpdateInterval = Prop("UpdateInterval");
		
		var propCustomLightBoundsOrigin = Prop("CustomLightBoundsOrigin");
		var propCustomLightBoundsRadius = Prop("CustomLightBoundsRadius");
		
		var propCustomCascadeCount = Prop("CustomCascadeCount");
		
		var propScatterEnabled = Prop("ScatterEnabled");
		var propOvercastAffectsScatter = Prop("OvercastAffectsScatter");
		var propCustomScatterOvercast = Prop("CustomScatterOvercast");
		var propScatterColor = Prop("ScatterColor");
		var propScatterRamp = Prop("ScatterRamp");
		var propScatterAnimateNoise = Prop("ScatterAnimateNoise");
		var propScatterResolution = Prop("ScatterResolution");
		var propScatterSamplingQualities = Prop("ScatterSamplingQuality");
		var propScatterBlur = Prop("ScatterBlur");
		
		var propDebugView = Prop("DebugView");

		bool aOK = false;
		
		bool resourcesOK = false;
		
		if(propOccluderShader.objectReferenceValue == null ||
			(propOccluderShader.objectReferenceValue as Shader).name != Sunshine.OccluderShaderName)
		{
			EditorGUILayout.HelpBox("Occluder Shader missing/invalid. Please locate it below:", MessageType.Error);
			if(GUILayout.Button("Fix now"))
				propOccluderShader.objectReferenceValue = Shader.Find(Sunshine.OccluderShaderName);	
			propOccluderShader.objectReferenceValue = EditorGUILayout.ObjectField("Occluder Shader", propOccluderShader.objectReferenceValue, typeof(Shader), true);
		}
		else if(propPostScatterShader.objectReferenceValue == null ||
			(propPostScatterShader.objectReferenceValue as Shader).name != Sunshine.PostScatterShaderName)
		{
			EditorGUILayout.HelpBox("Postprocess Scatter Shader missing/invalid. Please locate it below:", MessageType.Error);
			if(GUILayout.Button("Fix now"))
				propPostScatterShader.objectReferenceValue = Shader.Find(Sunshine.PostScatterShaderName);	
			propPostScatterShader.objectReferenceValue = EditorGUILayout.ObjectField("Post Scatter Shader", propPostScatterShader.objectReferenceValue, typeof(Shader), true);
		}
		else if(propPostBlurShader.objectReferenceValue == null ||
			(propPostBlurShader.objectReferenceValue as Shader).name != Sunshine.PostBlurShaderName)
		{
			EditorGUILayout.HelpBox("Postprocess Blur Shader missing/invalid. Please locate it below:", MessageType.Error);
			if(GUILayout.Button("Fix now"))
				propPostBlurShader.objectReferenceValue = Shader.Find(Sunshine.PostBlurShaderName);	
			propPostBlurShader.objectReferenceValue = EditorGUILayout.ObjectField("Post Blur Shader", propPostBlurShader.objectReferenceValue, typeof(Shader), true);
		}
		else if(propPostDebugShader.objectReferenceValue == null ||
			(propPostDebugShader.objectReferenceValue as Shader).name != Sunshine.PostDebugShaderName)
		{
			EditorGUILayout.HelpBox("Postprocess Debug Shader missing/invalid. Please locate it below:", MessageType.Error);
			if(GUILayout.Button("Fix now"))
				propPostDebugShader.objectReferenceValue = Shader.Find(Sunshine.PostDebugShaderName);	
			propPostDebugShader.objectReferenceValue = EditorGUILayout.ObjectField("Post Debug Shader", propPostDebugShader.objectReferenceValue, typeof(Shader), true);
		}
		else if(propBlankOvercastTexture.objectReferenceValue == null)
		{
			EditorGUILayout.HelpBox("Blank Overcast Texture missing. Please locate it below:", MessageType.Error);
			propBlankOvercastTexture.objectReferenceValue = EditorGUILayout.ObjectField("Blank Overcast Texture", propBlankOvercastTexture.objectReferenceValue, typeof(Shader), true);
		}
		else
			resourcesOK = true;
		
		if(resourcesOK)
		{
			GUILayout.Label("General Settings:");
			SunshineEditorHelper.EnableGroup(false, ()=>
			{
				string versionString = string.Format("Version {0}", Sunshine.Version);
				GUI.Label(SunshineEditorHelper.RightAlginedLabelOverlayRect(versionString), versionString);
			});
			SunshineEditorHelper.Box(()=>
			{
				Light sunLight = null;
				
				if(GameObject.FindObjectOfType(typeof(SunshineCamera)) == null)
				{
					aOK = false;
					SunshineEditorHelper.Horizontal(()=>
					{
						Camera cam = Camera.main;
						if(cam != null)
						{
							EditorGUILayout.HelpBox("No SunshineCamera components found.\nPlease attach SunshineCamera to your Main Camera(s)", MessageType.Warning);
							SunshineEditorHelper.Vertical(()=>
							{
								GUILayout.Space(12);
								if(GUILayout.Button("Fix Now"))
									cam.gameObject.AddComponent<SunshineCamera>();
							});
						}
					});
				}
				propSunLight.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("Sun Light", "Primary Directional Light in the Scene"), propSunLight.objectReferenceValue, typeof(Light), true);
				sunLight = propSunLight.objectReferenceValue as Light;
				if(!sunLight)
				{
					SunshineEditorHelper.Horizontal(()=>
					{
						Light newLight = sun.FindAppropriateSunLight();
						if(newLight)
						{
							aOK = true; //It's okay, we'll select it at runtime...
							EditorGUILayout.HelpBox("No Directional Light selected. Please select one.", MessageType.Warning);
							SunshineEditorHelper.Vertical(()=>
							{
								GUILayout.Space(12);
								if(GUILayout.Button("Fix Now"))
									propSunLight.objectReferenceValue = sunLight = newLight;
							});
						}
						else
						{
							EditorGUILayout.HelpBox("No appropriate Directional light found.  Please create one.", MessageType.Error);
							aOK = false;
						}
					});
				}
				else if(sunLight.type != LightType.Directional)
					EditorGUILayout.HelpBox("The Sun Light must be a Directional Light!", MessageType.Error);
				else
					aOK = true;
				
				{
					var layerNames = new List<string>();
					for(int i=31; i>=0; i--)
					{
						string name = LayerMask.LayerToName(i);
						if(!string.IsNullOrEmpty(name) || layerNames.Count > 0)
							layerNames.Insert(0, name);
					}
					
					propUsePerCascadeOccluders.boolValue = (sun.CascadeCount > 1) ? EditorGUILayout.Toggle(new GUIContent("Occluders Per-Cascade", "Specify a separate Occluder Mask for each Light Cascade"), propUsePerCascadeOccluders.boolValue) : false;
					for(int i=0; i<(propUsePerCascadeOccluders.boolValue ? sun.CascadeCount : 1); i++)
					{
						SunshineEditorHelper.Horizontal(()=>
						{
							propOccluders[i].intValue = EditorGUILayout.MaskField(new GUIContent(propUsePerCascadeOccluders.boolValue ? string.Format("Cascade {0} Occluders", i+1) : "Occluders", "Specifies layers that Occlude Light"), propOccluders[i].intValue, layerNames.ToArray());
							if(i>0 && GUILayout.Button(new GUIContent("^", "Copy Cascade 1 Occluders"), GUILayout.Width(20)))
								propOccluders[i].intValue = propOccluders[0].intValue;
						});
						if(propOccluders[i].intValue == 0)
							EditorGUILayout.HelpBox("\"Nothing\" casting shadows isn't going to do anything!", MessageType.Warning);
					}
				}
				#if UNITY_ANDROID || UNITY_IPHONE || UNITY_BLACKBERRY
				if(propOccluders[0].intValue == -1 && !sun.UsingCustomBounds)
					EditorGUILayout.HelpBox("\"Everything\" occluding light on Mobile may run slowly.\nConsider creating a layer for non-occluders!", MessageType.Warning);
				#endif
				
				propResolution.enumValueIndex = (int)(SunshineLightResolutions)EditorGUILayout.EnumPopup(new GUIContent("Light Resolution", "Resolution of Lightmap"), (SunshineLightResolutions)propResolution.enumValueIndex);
				#if UNITY_ANDROID || UNITY_IPHONE || UNITY_BLACKBERRY
				if(propResolution.enumValueIndex >= (int)SunshineLightResolutions.HighResolution && propResolution.enumValueIndex != (int)SunshineLightResolutions.Custom)
					EditorGUILayout.HelpBox("High resolutions on Mobile may run slowly\nConsider a lower resolution!", MessageType.Warning);
				#endif
				
				//Force a custom resolution when using Custom Bounds...
				if(sun.UsingCustomBounds && (SunshineLightResolutions)propResolution.enumValueIndex != SunshineLightResolutions.Custom)
				{
					propResolution.enumValueIndex = (int)SunshineLightResolutions.Custom;
					propCustomResolution.intValue = 256;
				}
				
				if((SunshineLightResolutions)propResolution.enumValueIndex == SunshineLightResolutions.Custom)
				{
					SunshineEditorHelper.Indent(()=>
					{
						EditorGUILayout.IntSlider(propCustomResolution, 1, 4096, new GUIContent("Custom Resolution", "Specifies the exact dimensions of the Lightmap"));
						if(!Mathf.IsPowerOfTwo(propCustomResolution.intValue))
						{
							SunshineEditorHelper.Horizontal(()=>
							{
								EditorGUILayout.HelpBox("A Power-of-Two Resolution is highly recommended.", MessageType.Warning);	
								SunshineEditorHelper.Vertical(GUILayout.Width(150), ()=>
								{
									GUILayout.Space(12);
									if(GUILayout.Button(new GUIContent("Make Power-of-Two", "Sets Custom Resolution to the next Power-of-Two")))
										propCustomResolution.intValue = Mathf.NextPowerOfTwo(propCustomResolution.intValue);
								});
							});
							EditorGUILayout.Space();
						}
					});
				}
				
				EditorGUILayout.Slider(Prop("CustomLightDistance"), 1f, 1000f, new GUIContent("Light Distance", "Distance from the camera that the Lightmap covers"));
				if(SunshineProjectPreferences.Instance.UseCustomShadows && !sun.UsingCustomBounds)
				{
					propCustomCascadeCount.intValue = SunshineEditorHelper.CascadeIntFromCount((SunshineCascadeCounts)EditorGUILayout.EnumPopup(new GUIContent("Cascade Count", "Number of Lightmap Cascades to create.\nCascades improve quality, but decrease performance"), SunshineEditorHelper.CascadeCountFromInt(propCustomCascadeCount.intValue)));
					if(sun.CascadeCount > 1)
					{
						var propUseManualCascadeSplits = Prop("UseManualCascadeSplits");
						propUseManualCascadeSplits.boolValue = (sun.CascadeCount > 2) && EditorGUILayout.Toggle("Manual Cascade Splits", propUseManualCascadeSplits.boolValue);					
						if(propUseManualCascadeSplits.boolValue)
						{
							EditorGUILayout.Slider(Prop("ManualCascadeSplit0"), 0.1f, 0.5f, "Cascade Split 1");
							if(sun.CascadeCount > 2)
								EditorGUILayout.Slider(Prop("ManualCascadeSplit1"), 0.1f, 0.5f, "Cascade Split 2");
							if(sun.CascadeCount > 3)
								EditorGUILayout.Slider(Prop("ManualCascadeSplit2"), 0.1f, 0.5f, "Cascade Split 3");
						}
						else
					{
							EditorGUILayout.Slider(Prop("CascadeSpacing"), 0.1f, 0.5f, new GUIContent("Cascade Spacing", "Ratio of each cascade's distance to the next"));
						}
						EditorGUILayout.Slider(Prop("CascadeFade"), 0f, 0.25f, new GUIContent("Cascade Fade", "Amount to transition between cascades"));
					}
				}
				
				var debugView = (SunshineDebugViews)EditorGUILayout.EnumPopup("Debug View", (SunshineDebugViews)propDebugView.enumValueIndex);;
				if(!propScatterEnabled.boolValue && debugView == SunshineDebugViews.Scatter)
					debugView = SunshineDebugViews.None;
				propDebugView.enumValueIndex = (int)debugView;
				
				ShowAdvanced = SunshineEditorHelper.Foldout(ShowAdvanced, new GUIContent("Advanced Settings", ""), ()=>
				{
					SunshineEditorHelper.Indent(()=>
					{
						propUseLODFix.boolValue = EditorGUILayout.Toggle(new GUIContent("LOD Fix", "Attempts to fix LOD mismatching"), propUseLODFix.boolValue);
						propUseOcclusionCulling.boolValue = EditorGUILayout.Toggle(new GUIContent("Occlusion Culling", "Enables Occlusion Culling on Lighmap Cameras"), propUseOcclusionCulling.boolValue);

						EditorGUILayout.Slider(Prop("LightPaddingZ"), 0f, 1000f, new GUIContent("Padding Z", "Moves Light cameras back to ensure Occluders behind the camera are captured."));
						propUpdateInterval.enumValueIndex = (int)(SunshineUpdateInterval)EditorGUILayout.EnumPopup(new GUIContent("Refresh Interval", "How often the Lightmap will be refreshed"), (SunshineUpdateInterval)propUpdateInterval.enumValueIndex);
						if(propUpdateInterval.enumValueIndex == (int)SunshineUpdateInterval.AfterXFrames)
						{
							SunshineEditorHelper.Indent(()=>
							{
								EditorGUILayout.IntSlider(Prop("UpdateIntervalFrames"), 1, 100, new GUIContent("Frame Interval", "Refreshes the Lightmap every X Frames.\nExample: A value of 2 will refresh every other frame."));
								EditorGUILayout.Slider(Prop("UpdateIntervalPadding"), 0.1f, 10f, new GUIContent("Bounds Padding", "Increases the Light volume to compensate for movement between refreshes"));
							});
						}
						else if(propUpdateInterval.enumValueIndex == (int)SunshineUpdateInterval.AfterXMovement)
						{
							SunshineEditorHelper.Indent(()=>
							{
								EditorGUILayout.Slider(Prop("UpdateIntervalMovement"), 0.1f, 10f, new GUIContent("Movement Interval", "Distance the camera needs to move to trigger a Lightmap refresh"));
							});
						}
						else if(propUpdateInterval.enumValueIndex == (int)SunshineUpdateInterval.Manual)
						{
							SunshineEditorHelper.Indent(()=>
							{
								EditorGUILayout.HelpBox("The Lightmap will ONLY be updated when SunshineCamera.RequestRefresh() is called!", MessageType.Info);
							});
						}

						propCustomLightBoundsOrigin.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("Custom Bounds", "Specify the position and radius the Lightmap should cover.\nPossible Uses:\n* Runtime \"baking\" of lightmaps\n* Player-only shadows on mobile"), propCustomLightBoundsOrigin.objectReferenceValue, typeof(Transform), true);
						if(propCustomLightBoundsOrigin.objectReferenceValue)
						{
							SunshineEditorHelper.Indent(()=>
							{
								EditorGUILayout.Slider(propCustomLightBoundsRadius, 0.1f, 1000f, new GUIContent("Custom Radius", "The radius around Custom Bounds that the Lightmap should cover"));
							});
						}
					});
				});
			});
			
			propScatterEnabled.boolValue = SunshineEditorHelper.ToggleGroupHidden(new GUIContent("Volumetric Scattering", "Simulates the effect of light on particulates in the air"), propScatterEnabled.boolValue, ()=>
			{
				SunshineEditorHelper.Box(()=>
				{
					propScatterColor.colorValue = EditorGUILayout.ColorField(new GUIContent("Color", "Color of particulates when illuminated"), propScatterColor.colorValue);
					EditorGUILayout.Slider(Prop("ScatterIntensity"), 0f, 0.75f, new GUIContent("Intensity", "Maximum opacity of particulates"));
					EditorGUILayout.Slider(Prop("ScatterExaggeration"), 0f, 1f, new GUIContent("Exaggeration", "Decreases the illuminated volume required for maximum opacity"));
					EditorGUILayout.Slider(Prop("ScatterSky"), 0f, 1f, new GUIContent("Sky Intensity", "Amount to apply on Sky pixels.\nDecrease if your scene looks \"washed out\"."));
					propScatterRamp.objectReferenceValue = EditorGUILayout.ObjectField("Ramp Texture", propScatterRamp.objectReferenceValue, typeof(Texture2D), true);

					propScatterResolution.enumValueIndex = (int)(SunshineRelativeResolutions)EditorGUILayout.EnumPopup(new GUIContent("Resolution", "Relative screen resolution for calculations"), (SunshineRelativeResolutions)propScatterResolution.enumValueIndex);
					
					propScatterSamplingQualities.enumValueIndex = (int)(SunshineScatterSamplingQualities)EditorGUILayout.EnumPopup(new GUIContent("Sample Quality", "Amount of ray-marching samples per-texel."), (SunshineScatterSamplingQualities)propScatterSamplingQualities.enumValueIndex);
					propScatterBlur.boolValue = EditorGUILayout.Toggle(new GUIContent("Blur Samples", "Performs a depth-aware blur of samples\nThis removes the appearance of stippling."), propScatterBlur.boolValue);
					EditorGUILayout.Slider(Prop("ScatterBlurDepthTollerance"), 0.01f, 1.0f, new GUIContent("Blur Depth Tollerance", "Controls how much depth discrepency is allowed to blur neighboring texels together."));
					propScatterAnimateNoise.boolValue = EditorGUILayout.Toggle(new GUIContent("Animate Dither", "Pallette-fades the dithering pattern\nFeeling nostalgic?"), propScatterAnimateNoise.boolValue);
					if(propScatterAnimateNoise.boolValue)
						EditorGUILayout.Slider(Prop("ScatterAnimateNoiseSpeed"), 0.01f, 4.0f, new GUIContent("Animate Speed", "Speed to animate dither.\n* Slower speeds feel like \"dust\"\n* Faster speeds feel like \"noise\""));
					
					propOvercastAffectsScatter.boolValue = EditorGUILayout.Toggle(new GUIContent("Use Overcast", "Control the density of particulates with a cookie.\nPossible Uses:\n* Make light appear to stream in through the clouds\n* Make light rays dance"), propOvercastAffectsScatter.boolValue);
					if(propOvercastAffectsScatter.boolValue)
					{
						SunshineEditorHelper.Indent(()=>
						{
							if(SunshineProjectPreferences.Instance.UseCustomShadows)
								propCustomScatterOvercast.boolValue = !EditorGUILayout.Toggle(new GUIContent("Inherit From Shadows", "Copy directly from your Shadow Overcast settings"), !propCustomScatterOvercast.boolValue);
							else
								propCustomScatterOvercast.boolValue = true;
						});
					}
					if(propOvercastAffectsScatter.boolValue && propCustomScatterOvercast.boolValue)
						OvercastSettings("ScatterOvercast");
				});
			});
			
			if(!SunshineProjectPreferences.Instance.UseCustomShadows)
			{
				ShowForwardShaders = false;
			}
			SunshineProjectPreferences.Instance.UseCustomShadows = SunshineEditorHelper.ToggleGroupHidden(new GUIContent("Custom Shadow Mapping", "Replaces Built-in Directional Shadow Mapping"), SunshineProjectPreferences.Instance.UseCustomShadows, ()=>
			{
				SunshineEditorHelper.Box(()=>
				{
					
					ShowForwardShaders = SunshineEditorHelper.Foldout(ShowForwardShaders, new GUIContent("Forward Renderer Support", "Tools to assist in swapping out shaders for use in the Forward Renderer\nThis is not needed in the Deferred Renderer!"), ()=>{});
					string labelText = "";
					Color labelColor = Color.green;
					if(SunshineProjectPreferences.Instance.ManualShaderInstallation)
						labelText = "Forward Shaders: Manual";
					else if(SunshineProjectPreferences.Instance.ForwardShadersInstalled)
						labelText = "Forward Shaders: Installed";
					else
					{
						labelText = "Deferred Support Only";
						labelColor = Color.yellow;
					}
					Rect labelRect = SunshineEditorHelper.RightAlginedLabelOverlayRect(labelText);
					SunshineEditorHelper.ColorGroup(labelColor, ()=> { GUI.Label(labelRect, labelText); });

					if(ShowForwardShaders)
					{
						SunshineEditorHelper.Indent(()=>
						{
							SunshineEditorHelper.EnableGroup(!Application.isPlaying, ()=>
							{
								SunshineProjectPreferences.Instance.ManualShaderInstallation = EditorGUILayout.Toggle(new GUIContent("Manual Installation", "Indicates you will point your materials to Sunshine shaders manually"), SunshineProjectPreferences.Instance.ManualShaderInstallation);
							});
							if(SunshineProjectPreferences.Instance.ManualShaderInstallation)
							{
									EditorGUILayout.HelpBox("To *Receive* Forward Shadows, your Materials must point to \"Sunshine/\" shaders.", MessageType.Info);
							}
							else
							{
								if(!SunshineProjectPreferences.Instance.ForwardShadersInstalled)
								{
									EditorGUILayout.HelpBox(
										"To *Receive* Forward Shadows, your Materials must point to Sunshine shaders.\n" +
										"For example, \"Diffuse\" will become \"Sunshine/Diffuse\".\n" +
										"This is a one-time step, and subsiquently imported materials will be automatically corrected."
										, MessageType.Info);
								}
								SunshineEditorHelper.EnableGroup(!Application.isPlaying, ()=>
								{
									SunshineEditorHelper.Horizontal(()=>
									{
										GUILayout.Space(150);
										if(!SunshineProjectPreferences.Instance.ForwardShadersInstalled)
										{
											if(GUILayout.Button(new GUIContent("Install Forward Shaders", "Automatically points your materials to Sunshine shaders if possible")))
												SunshineEditorHelper.Install();
											GUILayout.FlexibleSpace();
											if(GUILayout.Button(new GUIContent("Force Uninstall", "Automatically points your materials to non-Sunshine shaders.")))
												SunshineEditorHelper.Uninstall();
										}
										else
										{
											GUILayout.FlexibleSpace();
											if(GUILayout.Button(new GUIContent("Uninstall Forward Shaders", "Automatically points your materials to non-Sunshine shaders.")))
												SunshineEditorHelper.Uninstall();
										}
									});
								});
							}
						});
					}
					EditorGUILayout.Space();
					
					
					
					SunshineEditorHelper.EnableGroup(aOK, ()=>
					{
	
						if(propShadowFilter.enumValueIndex < 0)
							propShadowFilter.enumValueIndex = 2;
						if(propShadowFilter.enumValueIndex > 3)
							propShadowFilter.enumValueIndex = 3;
						propShadowFilter.enumValueIndex = (int)(SunshineShadowFilters)EditorGUILayout.EnumPopup(new GUIContent("Shadow Filter", "Type of filtering to apply to shadows"), (SunshineShadowFilters)propShadowFilter.enumValueIndex);
						#if UNITY_ANDROID || UNITY_IPHONE || UNITY_BLACKBERRY
						if(propShadowFilter.enumValueIndex >= (int)SunshineShadowFilters.PCF2x2)
							EditorGUILayout.HelpBox("Shadow filtering on Mobile may run slowly\nConsider Hard shadows", MessageType.Warning);
						#endif
						
						OvercastSettings("Overcast");
						
						EditorGUILayout.Slider(Prop("LightFadeRatio"), 0f, 1f, new GUIContent("Fade Ratio", "Ratio of Light Distance to fade out shadows"));
						EditorGUILayout.Slider(Prop("TerrainLODTweak"), 0f, 1f, new GUIContent("Terrain LOD Tweak", "Attempts to aleviate self-shadowing artifacts on Terrain by increasing LOD bias"));
						propShaderSet.enumValueIndex = (int)(SunshineShaderSets)EditorGUILayout.EnumPopup(new GUIContent("Shader Set", "Defines the minimum requirements for shadows.\n* Desktop requires Shader Model 3.0\n* Mobile requires Shader Model 2.0"), (SunshineShaderSets)propShaderSet.enumValueIndex);
						
					});
				});
			});
			if(!SunshineProjectPreferences.Instance.UseCustomShadows && SunshineProjectPreferences.Instance.ForwardShadersInstalled)
			{
				EditorGUILayout.HelpBox("Forward shaders are installed, but you aren't using Custom Shadows!", MessageType.Warning);
				if(GUILayout.Button(new GUIContent("Uninstall Forward Shaders", "Automatically points your materials to non-Sunshine shaders.")))
					SunshineEditorHelper.Uninstall();
			}
		}
		SunshineEditorHelper.PopColor();
		
		if(!sun.ScatterEnabled && !SunshineProjectPreferences.Instance.UseCustomShadows)
		{
			EditorGUILayout.HelpBox("There's nothing for Sunshine to do!\nEnable Volumetric Scattering, or Custom Shadow Mapping", MessageType.Warning);
		}
		else if(sun.Lightmap && sun.PostDebugMaterial && sun.PostDebugMaterial.SetPass(SunshinePostDebugPass.DebugLightmap))
		{
			DisplayLightmap = SunshineEditorHelper.Foldout(DisplayLightmap, new GUIContent(string.Format("Visualize Lightmap ({0}x{1})", sun.Lightmap.width, sun.Lightmap.height), ""), ()=>
			{
				float padding = 5;
				float size = 256;
				Rect rect = GUILayoutUtility.GetRect(size + padding * 2, size + padding * 2);
				Rect boxRect = rect;
				boxRect.y += padding;
				boxRect.height -= padding;
				boxRect.width = boxRect.height * sun.Lightmap.width / sun.Lightmap.height - padding;
				boxRect.x = (rect.width-boxRect.width) / 2;
				GUI.Box(boxRect, "");
				Rect textureRect = boxRect;
				textureRect.xMin += padding;
				textureRect.yMin += padding;
				textureRect.xMax -= padding;
				textureRect.yMax -= padding;
				EditorGUI.DrawPreviewTexture(textureRect, sun.Lightmap, sun.PostDebugMaterial);
				
				Repaint();
			});
		}
		serializedObject.ApplyModifiedProperties();
		SunshineProjectPreferences.Instance.SaveIfDirty();
	}
		
	
}
