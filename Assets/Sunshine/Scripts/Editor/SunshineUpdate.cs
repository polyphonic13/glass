using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

[InitializeOnLoad]
public class SunshineUpdate
{
    static SunshineUpdate()
    {
			EditorApplication.update += FinishedLoading;
	}

	static void DeleteDirectoryIfExists(string path)
	{
		if(Directory.Exists(path))
		{
			Debug.Log(string.Format("Sunshine: Deleting '{0}'", path));
			AssetDatabase.DeleteAsset(path);
		}
	}
	static void FinishedLoading()
	{
		if(SunshineProjectPreferences.Instance.Version != Sunshine.Version)
		{
			Debug.Log("Sunshine: Upgrading...");
			SunshineProjectPreferences.Instance.Version = Sunshine.Version;
			DeleteDirectoryIfExists("Assets/Sunshine/Shaders/Built-In Overrides");
			DeleteDirectoryIfExists("Assets/Sunshine/Shaders/Built-In Fallbacks");
			SunshineProjectPreferences.Instance.SaveIfDirty();
		}
		
		EditorApplication.update -= FinishedLoading;
	}
}
