using System;
using System.Collections;

namespace Polyworks
{
	[Serializable]
	public class SceneData
	{
		public string sceneName;
		public bool isPlayerLevel;
		public bool isCleared;

		public Prefab[] prefabs;
		public Task[] tasks;
	}
}

