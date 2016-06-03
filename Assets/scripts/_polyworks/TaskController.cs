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
		private Hashtable _countTasks;
		private Hashtable _valueTasks;
		private Hashtable _goalTasks;

		private int _countTasksCompleted = 0;
		private int _valueTasksCompleted = 0;
		private int _goalTasksCompleted = 0;

		private bool _isCountTasksCompleted = false; 
		private bool _isValueTasksCompleted = false; 
		private bool _isGoalTasksCompleted = false; 

		private Hashtable _gameDataTasks; 
		#endregion

		#region initialization
		public void Init(SceneData d) {
			_initCountTasks (d.countTasks);
			_initValueTasks (d.valueTasks);
			_initGoalTasks (d.goalTasks);
		}

		private void _initCountTasks(CountTaskData[] data) {
			_countTasks = new Hashtable ();
			for (int i = 0; i < data.Length; i++) {
				_countTasks.Add(data[i].name, new CountTask(data[i]));
				if (data [i].isCompleted) {
					_countTasksCompleted++;
				}
			}
			if (_countTasksCompleted == data.Length) {
				_allTasksCompletedCheck ();
			}
		}

		private void _initValueTasks(ValueTaskData[] data) {
			_valueTasks = new Hashtable ();
			for (int i = 0; i < data.Length; i++) {
				_valueTasks.Add(data[i].name, new ValueTask(data[i]));
				if (data [i].isCompleted) {
					_valueTasksCompleted++;
				}
			}
			if (_valueTasksCompleted == data.Length) {
				_allTasksCompletedCheck ();
			}
		}

		private void _initGoalTasks(GoalTaskData[] data) {
			_goalTasks = new Hashtable ();
			for (int i = 0; i < data.Length; i++) {
				_goalTasks.Add(data[i].name, new GoalTask(data[i]));
				if (data [i].isCompleted) {
					_goalTasksCompleted++;
				}
			}
			if (_goalTasksCompleted == data.Length) {
				_allTasksCompletedCheck ();
			}
		}
		#endregion

		#region handlers
		public void OnCountTaskUpdated(string name, int value) {
			if (_countTasks.Contains (name)) {
				CountTask task = _countTasks [name] as CountTask;
				task.Update ();

				if (task.data.isCompleted) {
					_countTasksCompleted++;

					if (_countTasksCompleted == _countTasks.Count) {
						_allTasksCompletedCheck ();
					}
				}
			}
		}

		public void OnValueTaskUpdated(string name, float value) {
			if (_valueTasks.Contains (name)) {
				ValueTask task = _valueTasks [name] as ValueTask;
				task.Update (value);

				if (task.data.isCompleted) {
					_valueTasksCompleted++;

					if (_valueTasksCompleted == _valueTasks.Count) {
						_allTasksCompletedCheck ();
					}
				}
			}
		}

		public void OnCountTaskUpdated(string name, string value) {
			if (_goalTasks.Contains (name)) {
				GoalTask task = _goalTasks [name] as GoalTask;
				task.Update (value);

				if (task.data.isCompleted) {
					_goalTasksCompleted++;

					if (_goalTasksCompleted == _goalTasks.Count) {
						_allTasksCompletedCheck ();
					}
				}
			}
		}
		#endregion

		private void _allTasksCompletedCheck() {
			if (_isCountTasksCompleted && _isValueTasksCompleted && _isGoalTasksCompleted) {
				EventCenter.Instance.UpdateSceneTasksCompleted ();
			}
		}
	}

	#region task definitions
	public class Task {

		public TaskData data;

	}

	public class CountTask: Task {
		public CountTaskData data; 

		public CountTask(CountTaskData d) {
			data = d;
		}

		public void Update() {
			data.current++;
			if (data.current == data.goal) {
				data.isCompleted = true;
			}
		}
	}

	public class ValueTask: Task {
		public ValueTaskData data; 

		public ValueTask(ValueTaskData d) {
			data = d;
		}

		public void Update(float val) {
			data.current = val;
			if (data.current == data.goal) {
				data.isCompleted = true;
			}
		}
	}

	public class GoalTask: Task {
		public GoalTaskData data; 

		public GoalTask(GoalTaskData d) {
			data = d;
		}

		public void Update(string val) {
			data.current = val;
			if (data.current == data.goal) {
				data.isCompleted = true;
			}
		}
	}
	#endregion
}

