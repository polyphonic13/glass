using UnityEngine;
using UnityEditor;
using System.Collections;

public class SunshineMaterialProcessor : AssetPostprocessor
{
	static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath)
	{
		if(SunshineEditorHelper.SunshineIsProcessingMaterials || importedAssets.Length <= 0)
			return;
		if(!SunshineProjectPreferences.Instance.UseCustomShadows)
			return;
		if(SunshineProjectPreferences.Instance.ManualShaderInstallation)
			return;
		SunshineEditorHelper.SwapMaterialShadersByPaths(importedAssets, SunshineProjectPreferences.Instance.ForwardShadersInstalled, false);
	}
}
