using System;
using System.Collections.Generic; 

namespace Polyworks
{
	[Serializable]
	public class TaskData
	{
		public string name;
		public bool isCompleted { get; set; }
	}

	[Serializable]
	public class CountTaskData: TaskData {
		public int goal;
		public int current { get; set; }

		public CountTaskData() {}

		public CountTaskData Clone() {
			CountTaskData clone = new CountTaskData ();
			clone.name = this.name;
			clone.isCompleted = this.isCompleted;
			clone.goal = this.goal;
			clone.current = this.current;
			return clone;
		}
	}

	[Serializable]
	public class ValueTaskData: TaskData {
		public float goal;
		public float current { get; set; }

		public ValueTaskData() {}

		public ValueTaskData Clone() {
			ValueTaskData clone = new ValueTaskData ();
			clone.name = this.name;
			clone.isCompleted = this.isCompleted;
			clone.goal = this.goal;
			clone.current = this.current;
			return clone;
		}
	}

	[Serializable]
	public class GoalTaskData: TaskData {
		public string goal;
		public string current { get; set; }

		public GoalTaskData() {}

		public GoalTaskData Clone() {
			GoalTaskData clone = new GoalTaskData ();
			clone.name = this.name;
			clone.isCompleted = this.isCompleted;
			clone.goal = this.goal;
			clone.current = this.current;
			return clone;
		}
	}
}

