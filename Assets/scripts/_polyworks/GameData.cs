using UnityEngine;
using System.Collections;

namespace Polyworks {
	[System.Serializable]
	public class GameData {
		public string objectName = "serialized_data";
		public string currentScene;
		public string currentTargetScene; 

		public int count = 0;

		public Hashtable items;

//		public ScenePrefabCollection[] scenePrefabs;
	}

//	[System.Serializable]
//	public struct ScenePrefabCollection {
//		public string sceneName;
//		public Prefab[] prefabs;
//	}
//
//	[System.Serializable]
//	public struct Prefab {
//		public string name;
//		[SerializeField] public Vector3 location;
//		[SerializeField] public Quaternion rotation;
//	}
}

