using System;
using UnityEngine;

namespace Polyworks
{
	public class ScenePrefabData: MonoBehaviour {
		public string sceneName;
		public Prefab[] prefabs;
	}

	[Serializable]
	public struct Prefab {
		public string name;
		public Vector3 location;
		public Quaternion rotation;
	}
}

