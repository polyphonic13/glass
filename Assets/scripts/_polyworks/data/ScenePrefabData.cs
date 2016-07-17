using System;
using UnityEngine;

namespace Polyworks
{
	[Serializable]
	public class ScenePrefabData {
		public Prefab[] prefabs;
	}

	[Serializable]
	public struct Prefab {
		public string name;
		public string path;
		public string addTo;
		public Vector3 location;
		public Quaternion rotation;
	}

}

