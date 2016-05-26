using System;

namespace Polyworks
{
	[Serializable]
	public class TaskController
	{
		public static enum Types {
			COUNT,
			VALUE,
			GOAL
		};
		
		public CountTask[] countTasks;
		public ValueTask[] valueTasks;
		public GoalTask[] goalTasks;
		
		private bool _isComplete = false;
		private int _totalTasks;
		private int _completedTasks; 
		
		public TaskController() {
			
		}
		
		public void OnCountTaskUpdated(string task, int count) {
			CountTask task;
			for(int i = 0; i < countTasks.Count; i++) {
				if(countTasks[i].name == task) {
					task = countTasks[i];
					break;
				}
			}
			
			if(task != null) {
				task.count++;
				if(task.count >= task.total) {
					GameController.Instance.CompleteCountTask(task.name);
				}
			}
		}
		
	}
	
	public class Task {
		public string name;
		public string scene;
		
		public bool isComplete { get; set; }
	}
	
	public class CountTask: Task {
		public int count;
		public int total;
		public TaskController.Types type = TaskController.Types.COUNT;
	}
	
	public class ValueTask: Task {
		public float value; 
		public float total;
		public TaskController.Types type = TaskController.Types.VALUE;
	}

	public class GoalTask: Task {
		public string goal; 
		public TaskController.Types type = TaskController.Types.GOAL;
	}
}

