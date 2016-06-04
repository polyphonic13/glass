using System;
using System.Collections;

namespace Polyworks
{
	[Serializable]
	public class SceneData
	{
		public string sceneName;
		public bool isPlayerLevel;

		public CountTaskData[] countTasks; 
		public ValueTaskData[] valueTasks;
		public GoalTaskData[] goalTasks;

		public Prefab[] prefabs;

	}
}

