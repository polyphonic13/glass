using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.IO;
using System;

public class SunshineProjectPreferences : ScriptableObject
{
		public const string AssetPath = "Assets/Sunshine/Resources/SunshinePreferences.asset";
		public const string ResourceName = "SunshinePreferences";

		private static SunshineProjectPreferences instance = null;

		public static SunshineProjectPreferences Instance {
				get {
						if (instance == null)
								instance = Load ();
						return instance;
				}
		}

		private static SunshineProjectPreferences Load ()
		{		
				SunshineProjectPreferences prefs = null;
				try {
						prefs = Resources.Load (ResourceName, typeof(SunshineProjectPreferences)) as SunshineProjectPreferences;
				} catch {
				}

				if (prefs == null) {
						prefs = ScriptableObject.CreateInstance<SunshineProjectPreferences> ();
						prefs.name = "Sunshine Project Configuration";
						prefs.hideFlags = HideFlags.NotEditable;
				}
				return prefs;
		}

		public void SaveIfDirty ()
		{
#if UNITY_EDITOR
				if (isDirty) {
						try {
								EditorUtility.SetDirty (this);
								if (!AssetDatabase.Contains (this)) {
										var directoryName = Path.GetDirectoryName (AssetPath);
										if (!Directory.Exists (directoryName))
												Directory.CreateDirectory (directoryName);
										AssetDatabase.CreateAsset (this, AssetPath);
								} else
										AssetDatabase.SaveAssets ();
						} catch {
						}
						isDirty = false;
				}
#endif	
		}
		#if UNITY_EDITOR
		private bool isDirty = false;
		#endif
	
		[SerializeField]
		private string version = "";

		public string Version {
				get { return version; }
				set {
						if (version == value)
								return;
						version = value;
#if UNITY_EDITOR
						isDirty = true;
#endif
				}
		}

		[SerializeField]
		private bool forwardShadersInstalled = false;

		public bool ForwardShadersInstalled {
				get { return forwardShadersInstalled; }
				set {
						if (forwardShadersInstalled == value)
								return;
						forwardShadersInstalled = value;
#if UNITY_EDITOR
						isDirty = true;
#endif
				}
		}

		[SerializeField]
		private bool useCustomShadows = false;

		public bool UseCustomShadows {
				get { return useCustomShadows; }
				set {
						if (useCustomShadows == value)
								return;
						useCustomShadows = value;
#if UNITY_EDITOR
						isDirty = true;
#endif
				}
		}

		[SerializeField]
		private bool manualShaderInstallation = true;

		public bool ManualShaderInstallation {
				get { return manualShaderInstallation; }
				set {
						if (manualShaderInstallation == value)
								return;
						manualShaderInstallation = value;
#if UNITY_EDITOR
						isDirty = true;
#endif
				}
		}

		[SerializeField]
		private bool installerRunning = false;

		public bool InstallerRunning {
				get { return installerRunning; }
				set {
						if (installerRunning == value)
								return;
						installerRunning = value;
#if UNITY_EDITOR
						isDirty = true;
#endif
				}
		}
}