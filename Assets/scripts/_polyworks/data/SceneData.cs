using System;
using System.Collections;

namespace Polyworks
{
	[Serializable]
	public class SceneData
	{
		public string sceneName;
		public bool isPlayerLevel;
		public bool isCleared { get; set; }

		public CountTaskData[] countTasks; 
		public ValueTaskData[] valueTasks;
		public GoalTaskData[] goalTasks;

		public Prefab[] prefabs;
	}
}

