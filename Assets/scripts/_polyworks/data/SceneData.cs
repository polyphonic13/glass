using System;
using System.Collections;
using UnityEngine;

namespace Polyworks
{
	[Serializable]
	public class SceneData
	{
		public string sceneName;

		public Prefab[] prefabs;
		public PlayerLocation[] playerLocations;

	}

	[Serializable]
	public struct PlayerLocation {
		public Vector3 position;
		public Quaternion rotation;
	}

}

