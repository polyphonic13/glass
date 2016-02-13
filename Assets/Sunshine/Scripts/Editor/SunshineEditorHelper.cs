using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

public static class SunshineEditorHelper
{
	public static bool SunshineIsProcessingMaterials = false;
	
	private static Stack<Color> _ColorStack = new Stack<Color>();
	public static void PushColor(Color color)
	{
		_ColorStack.Push(GUI.color);
		GUI.color = color;
	}
	public static void PopColor()
	{
		GUI.color = _ColorStack.Pop();
	}
	public static void ClearColor()
	{
		_ColorStack.Clear();
	}
	
	public delegate void VoidDelegate();
	public static void EnableGroup(bool enabled, VoidDelegate callback)
	{
		EditorGUI.BeginDisabledGroup(!enabled);
		callback();
		EditorGUI.EndDisabledGroup();
	}
	public static bool ToggleGroupHidden(GUIContent label, bool toggle, VoidDelegate callback)
	{
		bool result = EditorGUILayout.BeginToggleGroup(label, toggle);
		EditorGUILayout.EndToggleGroup();
		if(result)
			callback();
		return result;
	}
	public static bool ToggleGroupDisabled(GUIContent label, bool toggle, VoidDelegate callback)
	{
		bool result = EditorGUILayout.BeginToggleGroup(label, toggle);
		callback();
		EditorGUILayout.EndToggleGroup();
		return result;
	}
	public static Rect RightAlginedLabelOverlayRect(string labelText)
	{
		Rect lastRect = GUILayoutUtility.GetLastRect();
		var labelSize = GUI.skin.GetStyle("Label").CalcSize(new GUIContent(labelText)) + new Vector2(5, 0);
		Rect labelRect = Rect.MinMaxRect(lastRect.xMax - labelSize.x, lastRect.y, lastRect.xMax, lastRect.yMax);
		return labelRect;
	}
	public static bool Foldout(bool toggle, GUIContent label, VoidDelegate callback)
	{
		var rect = GUILayoutUtility.GetRect(new GUIContent("\t" + label.text), GUIStyle.none);
		//EditorGUILayout.Space();
		//bool result = EditorGUILayout.Foldout(toggle, label);
		bool result = EditorGUI.Foldout(rect, toggle, label, true);
		if(result)
			callback();
		return result;
	}
	public static void Indent(VoidDelegate callback)
	{
		EditorGUI.indentLevel++;
		callback();
		EditorGUI.indentLevel--;
	}
	public static void Indent(int levels, VoidDelegate callback)
	{
		EditorGUI.indentLevel+=levels;
		callback();
		EditorGUI.indentLevel-=levels;
	}
	public static void Box(VoidDelegate callback)
	{
		Rect rect = EditorGUILayout.BeginVertical();
		GUI.Box(rect, "");
		EditorGUILayout.Space();

		callback();
		
		EditorGUILayout.Space();
		EditorGUILayout.EndVertical();
	}
	public static void Horizontal(VoidDelegate callback)
	{
		EditorGUILayout.BeginHorizontal();
		callback();
		EditorGUILayout.EndHorizontal();
	}
	public static void Vertical(VoidDelegate callback)
	{
		EditorGUILayout.BeginVertical();
		callback();
		EditorGUILayout.EndVertical();
	}
	public static void Vertical(GUILayoutOption option, VoidDelegate callback)
	{
		EditorGUILayout.BeginVertical(option);
		callback();
		EditorGUILayout.EndVertical();
	}
	public static void ColorGroup(Color color, VoidDelegate callback)
	{
		PushColor(color);
		callback();
		PopColor();
	}

	public static SunshineCascadeCounts CascadeCountFromInt(int cascadeCount)
	{
		SunshineCascadeCounts result = SunshineCascadeCounts.NoCascades;
		switch(cascadeCount)
		{
			case 2: result = SunshineCascadeCounts.TwoCascades; break;
			case 4: result = SunshineCascadeCounts.FourCascades; break;
		}
		return result;
	}
	public static int CascadeIntFromCount(SunshineCascadeCounts cascadeCount)
	{
		int result = 1;
		switch(cascadeCount)
		{
			case SunshineCascadeCounts.TwoCascades: result = 2; break;
			case SunshineCascadeCounts.FourCascades: result = 4; break;
		}
		return result;
	}	
	
	public static void Install() { InstallUninstall(true); }
	public static void Uninstall() { InstallUninstall(false); }
	private static void InstallUninstall(bool enableSunshine)
	{
		SunshineProjectPreferences.Instance.InstallerRunning = true;
		SunshineProjectPreferences.Instance.SaveIfDirty();
		var assetPaths = AssetDatabase.GetAllAssetPaths();
		if(SwapMaterialShadersByPaths(assetPaths, enableSunshine, true))
			SunshineProjectPreferences.Instance.ForwardShadersInstalled = enableSunshine;
		SunshineProjectPreferences.Instance.InstallerRunning = false;
		SunshineProjectPreferences.Instance.SaveIfDirty();
	}
	
	static void SaveAssetsAndFreeMemory()
	{
		AssetDatabase.SaveAssets();
		GC.Collect();
		EditorUtility.UnloadUnusedAssetsImmediate();
	}
	
	public static bool SwapMaterialShadersByPaths(string[] assetPaths, bool enableSunshine, bool displayProgress)
	{
		string shaderSet = enableSunshine ? "Sunshine" : "Regular";
		string taskDescription = string.Format(Sunshine.FormatMessage("Setting Materials to {0} Shaders"), shaderSet);
		
		SunshineIsProcessingMaterials = true;
		
		List<string> materialPaths = new List<string>();
		foreach(string assetPath in assetPaths)
		{
			if(assetPath.ToLower().EndsWith(".mat"))
			{
				string message = "";
				if(AssetDatabase.IsOpenForEdit(assetPath, out message))
					materialPaths.Add(assetPath);
				else
					Sunshine.LogMessage(string.Format("Unable to edit Material.\nFile: {0}\nReason: {1}", assetPath, message), true);
			}
		}

		int count = materialPaths.Count;
		if(count <= 0)
		{
			SunshineIsProcessingMaterials = false;
			return true;
		}
		
		bool canceled = false;
		for(int i=0; i<count; i++)
		{
			string materialPath = materialPaths[i];
			try
			{
				if(displayProgress && EditorUtility.DisplayCancelableProgressBar(taskDescription, materialPath, (float)i / (float)count))
				{
					canceled = true;
					break;
				}

				Material mat = AssetDatabase.LoadMainAssetAtPath(materialPath) as Material;
				if(mat != null)
				{
					SwapMaterialShader(mat, enableSunshine);
					mat = null;
				}
				SaveAssetsAndFreeMemory();
			}
			catch
			{
				Sunshine.LogMessage(string.Format("Problem Editing Material \"{0}\"", materialPath), true);
			}
		}
		SaveAssetsAndFreeMemory();


		if(displayProgress)
			EditorUtility.ClearProgressBar();
		
		SunshineIsProcessingMaterials = false;
		
		return !canceled;
	}
	static bool SwapMaterialShader(Material material, bool enableSunshine)
	{
		if(!material)
			return false;
		const string hiddenPrefix = "Hidden/";
		const string sunshinePrefix = "Sunshine/";
		
		string shaderName = material.shader.name;
		
		bool isHiddenShader = shaderName.StartsWith(hiddenPrefix);
		if(isHiddenShader) shaderName = shaderName.Substring(hiddenPrefix.Length);
		
		bool isSunshineShader = (shaderName.StartsWith(sunshinePrefix));
		if(isSunshineShader) shaderName = shaderName.Substring(sunshinePrefix.Length);

		string prefix = isHiddenShader ? hiddenPrefix : "";
		if(enableSunshine)		prefix += sunshinePrefix;
		
		string replacementShaderName = prefix + shaderName;
		if(replacementShaderName == material.shader.name)
		{
			return false;
		}
		
		Shader replacementShader = Shader.Find(replacementShaderName);
		if(replacementShader == null)
			return false;
		
		material.shader = replacementShader;
		
		return true;
	}
}
