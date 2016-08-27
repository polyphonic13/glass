using System;
using UnityEngine;

namespace Polyworks
{
	[Serializable]
	public class ScenePrefabData {
		public Prefab[] prefabs;
		public SectionPrefabs[] sectionPrefabs;
	}

	[Serializable]
	public struct Prefab {
		public string name;
		public string path;
		public string addTo;
		public Vector3 location;
		public Vector3 rotation;
	}

	[Serializable]
	public struct SectionPrefabs {
		public string name;
		public Prefab[] prefabs;
	}
}

