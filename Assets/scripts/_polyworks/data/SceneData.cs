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

		public TaskCollection<int> countTasks;
		public TaskCollection<float> valueTasks;
		public TaskCollection<string> goalTasks;

		public Prefab[] prefabs;
	}
}

