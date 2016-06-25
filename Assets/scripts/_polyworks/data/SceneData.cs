using System;
using System.Collections;

namespace Polyworks
{
	[Serializable]
	public class SceneData
	{
		public string sceneName;
		public bool isPlayerLevel;

		public SceneTaskData tasks; 

		public Prefab[] prefabs;

	}

}

