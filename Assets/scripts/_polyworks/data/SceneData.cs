using System;
using System.Collections;

namespace Polyworks
{
	[Serializable]
	public class SceneData
	{
		public string sceneName;
		public bool isPlayerLevel;

		public IntTaskData[] intTasks; 
		public FloatTaskData[] floatTasks;
		public StringTaskData[] stringTasks;

		public Prefab[] prefabs;

	}
}

