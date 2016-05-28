using System;
using UnityEngine;

namespace Polyworks
{
	public class TaskController: MonoBehaviour
	{
		[SerializeField] public CountTask[] countTasks;
		[SerializeField] public ValueTask[] valueTasks;
		[SerializeField] public GoalTask[] goalTasks;
		
		private bool _isComplete = false;
		private int _totalTasks;
		private int _completedTasks; 

		public TaskController() {
			
		}
		
		public void OnCountTaskUpdated(string name, int count) {
			CountTask task = _findTask(name, countTasks) as CountTask;

			if(task != null) {
				task.count++;
				if(task.count >= task.total) {
					task.isComplete = true;
					GameController.Instance.CompleteCountTask(task.name);
				}
			}
		}

		public void OnValueTaskUpdated(string name, float value) {
			ValueTask task = _findTask(name, countTasks) as ValueTask;

			if(task != null) {
				task.value += value;
				if(task.value >= task.total) {
					task.isComplete = true;
					GameController.Instance.CompleteValueTask(task.name);
				}
			}
		}

		public void OnGoalTaskUpdated(string name, string goal) {
			GoalTask task = _findTask(name, countTasks) as GoalTask;

			if(task != null && goal == task.goal) {
				task.isComplete = true;
				GameController.Instance.CompleteGoalTask(task.name);
			}
		}

		private Task _findTask(string name, Task[] tasks) {
			Task task = null;

			for(int i = 0; i < tasks.Length; i++) {
				if(tasks[i].name == name) {
					task = tasks[i];
					break;
				}
			}
			return task;
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
	}
	
	public class ValueTask: Task {
		public float value; 
		public float total;
	}

	public class GoalTask: Task {
		public string goal; 
	}
}

