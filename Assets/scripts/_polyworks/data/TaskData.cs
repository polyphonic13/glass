using System;
using System.Collections.Generic; 

namespace Polyworks
{
	[Serializable]
	public class LevelTaskData
	{
		public IntTaskData[] intTasks; 
		public FloatTaskData[] floatTasks;
		public StringTaskData[] stringTasks;
		public CollectionTaskData[] collectionTasks;
	}

	[Serializable]
	public class TaskData
	{
		public string name;
		public bool isCompleted = false;
	}

	[Serializable]
	public class IntTaskData: TaskData {
		public int goal;
		public int current = 0;

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
		public float current = 0;

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
		public string current = "";

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

	[Serializable]
	public class CollectionTaskData: TaskData {
		public int goal = 1;
		public int current = 0;

		public CollectionTaskData() {}

		public CollectionTaskData Clone() {
			CollectionTaskData clone = new CollectionTaskData ();
			clone.name = this.name;
			clone.isCompleted = this.isCompleted;
			clone.goal = this.goal;
			clone.current = this.current;
			return clone;
		}
	}
}

