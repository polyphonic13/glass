using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Polyworks
{
	[Serializable]
	public class TaskController: MonoBehaviour
	{
		#region members
		private CountTaskData[] _countTasks;
		private ValueTaskData[] _valueTasks;
		private GoalTaskData[] _goalTasks;

		private int _countTasksCompleted = 0;
		private int _valueTasksCompleted = 0;
		private int _goalTasksCompleted = 0;

		private bool _isCountTasksCompleted = false; 
		private bool _isValueTasksCompleted = false; 
		private bool _isGoalTasksCompleted = false; 

		private Hashtable _gameDataTasks; 
		#endregion

		#region public methods
		public void Init(SceneData sceneData, Hashtable taskData) {
			_countTasks = taskData["countTasks"] as CountTaskData[];
			_valueTasks = taskData["valueTasks"] as ValueTaskData[];
			_goalTasks = taskData["goalTasks"] as GoalTaskData[];
		}

		public Hashtable GetData() {
			Hashtable taskData = new Hashtable ();
			taskData.Add("countTasks", _countTasks);
			taskData.Add("valueTasks", _valueTasks);
			taskData.Add("goalTasks",  _goalTasks);
			return taskData;
		}
		#endregion

		#region handlers
		public void OnCountTaskUpdated(string name, int value) {
			CountTaskData task = _findTask (_countTasks, name) as CountTaskData;
			task.current = value;
			if (task.current == task.goal) {
				_taskCompleted (task, _countTasks);
			}
		}

		public void OnValueTaskUpdated(string name, float value) {
			ValueTaskData task = _findTask (_valueTasks, name) as ValueTaskData;
			task.current = value;
			if (task.current == task.goal) {
				_taskCompleted (task, _valueTasks);
			}
		}

		public void OnGoalTaskUpdated(string name, string value) {
			GoalTaskData task = _findTask (_goalTasks, name) as GoalTaskData;
			task.current = value;
			if (task.current == task.goal) {
				_taskCompleted (task, _goalTasks);
			}
		}
		#endregion

		#region private methods
		private TaskData _findTask(TaskData[] tasks, string name) {
			for(int i = 0; i < tasks.Length; i++) {
				if(tasks[i].name == name) {
					return tasks[i];
				}
			}
			return null;
		}				

		private void _taskCompleted(TaskData task, TaskData[] tasks) {
			task.isCompleted = true;

			bool isAllCompleted = true;
			for (int i = 0; i < tasks.Length; i++) {
				if (!tasks [i].isCompleted) {
					isAllCompleted = false;
					break;
				}
			}

			if (isAllCompleted) {
				_allTasksCompletedCheck ();
			}
		}

		private void _allTasksCompletedCheck() {
			if (_isCountTasksCompleted && _isValueTasksCompleted && _isGoalTasksCompleted) {
				EventCenter.Instance.UpdateSceneTasksCompleted ();
			}
		}
		#endregion
	}

}

