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
	public class IntTaskData: TaskData {
		public int goal;
		public int current { get; set; }

		public IntTaskData() {}

		public IntTaskData Clone() {
			IntTaskData clone = new IntTaskData ();
			clone.name = this.name;
			clone.isCompleted = this.isCompleted;
			clone.goal = this.goal;
			clone.current = this.current;
			return clone;
		}
	}

	[Serializable]
	public class FloatTaskData: TaskData {
		public float goal;
		public float current { get; set; }

		public FloatTaskData() {}

		public FloatTaskData Clone() {
			FloatTaskData clone = new FloatTaskData ();
			clone.name = this.name;
			clone.isCompleted = this.isCompleted;
			clone.goal = this.goal;
			clone.current = this.current;
			return clone;
		}
	}

	[Serializable]
	public class StringTaskData: TaskData {
		public string goal;
		public string current { get; set; }

		public StringTaskData() {}

		public StringTaskData Clone() {
			StringTaskData clone = new StringTaskData ();
			clone.name = this.name;
			clone.isCompleted = this.isCompleted;
			clone.goal = this.goal;
			clone.current = this.current;
			return clone;
		}
	}
}

