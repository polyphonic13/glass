using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SunshineRenderer))]
public class SunshineRendererEditor : Editor
{
	public override void OnInspectorGUI ()
	{
#if UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
		EditorGUILayout.HelpBox("This component allows Sunshine to respect a Renderer's \"Receieve Shadows\" option.", MessageType.None);
#else
		EditorGUILayout.HelpBox("This component requires Unity 4.1 or above.", MessageType.Warning);
#endif
	}
		
	
}
