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
		private IntTaskData[] _intTasks;
		private FloatTaskData[] _floatTasks;
		private StringTaskData[] _stringTasks;

		private int _intTasksCompleted = 0;
		private int _floatTasksCompleted = 0;
		private int _stringTasksCompleted = 0;

		private bool _isIntTasksCompleted = false; 
		private bool _isFloatTasksCompleted = false; 
		private bool _isStringTasksCompleted = false; 

		private Hashtable _gameDataTasks; 
		#endregion

		#region public methods
		public void Init(SceneData sceneData, Hashtable taskData) {
			Debug.Log ("TaskController/Init");
//			_intTasks = taskData["intTasks"] as IntTaskData[];
//			_floatTasks = taskData["floatTasks"] as FloatTaskData[];
//			_stringTasks = taskData["stringTasks"] as StringTaskData[];
			if (sceneData.intTasks.Length == 0) {
				_isIntTasksCompleted = true;
			} else {
				if (taskData != null && taskData.Contains ("intTasks")) {
					_intTasks = taskData ["intTasks"] as IntTaskData[];
				} else {
					_intTasks = sceneData.intTasks;
				}
			}

			if (sceneData.floatTasks.Length == 0) {
				_isFloatTasksCompleted = true;
			} else {
				if (taskData != null && taskData.Contains ("floatTasks")) {
					_floatTasks = taskData ["floatTasks"] as FloatTaskData[];
				} else {
					_floatTasks = sceneData.floatTasks;
				}
			}

			if (sceneData.stringTasks.Length == 0) {
				_isStringTasksCompleted = true;
			} else {
				if (taskData != null && taskData.Contains ("stringTasks")) {
					_stringTasks = taskData ["stringTasks"] as StringTaskData[];
				} else {
					_stringTasks = sceneData.stringTasks;
				}
			}

			EventCenter ec = EventCenter.Instance;
			ec.OnIntTaskUpdated += OnIntTaskUpdated;
			ec.OnFloatTaskUpdated += OnFloatTaskUpdated;
			ec.OnStringTaskUpdated += OnStringTaskUpdated;

		}

		public Hashtable GetData() {
			Hashtable taskData = new Hashtable ();
			taskData.Add("intTasks", _intTasks);
			taskData.Add("floatTasks", _floatTasks);
			taskData.Add("stringTasks",  _stringTasks);
			return taskData;
		}
		#endregion

		#region handlers
		public void OnIntTaskUpdated(string name, int value) {
			IntTaskData task = _findTask (_intTasks, name) as IntTaskData;
			task.current = value;
			if (task.current == task.goal) {
				_taskCompleted (task, _intTasks, out _isIntTasksCompleted);
			}
		}

		public void OnFloatTaskUpdated(string name, float value) {
			FloatTaskData task = _findTask (_floatTasks, name) as FloatTaskData;
			task.current = value;
			if (task.current == task.goal) {
				_taskCompleted (task, _floatTasks, out _isFloatTasksCompleted);
			}
		}

		public void OnStringTaskUpdated(string name, string value) {
			StringTaskData task = _findTask (_stringTasks, name) as StringTaskData;
			task.current = value;
			if (task.current == task.goal) {
				_taskCompleted (task, _stringTasks, out _isStringTasksCompleted);
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

		private void _taskCompleted(TaskData task, TaskData[] tasks, out bool isTypeCompleted) {
//			Debug.Log ("TaskController/_taskCompleted, task = " + task.name);
			task.isCompleted = true;

			isTypeCompleted = true;
			for (int i = 0; i < tasks.Length; i++) {
				if (!tasks [i].isCompleted) {
					isTypeCompleted = false;
					break;
				}
			}

			if (isTypeCompleted) {
				_allTasksCompletedCheck ();
			}
		}

		private void _allTasksCompletedCheck() {
			if (_isIntTasksCompleted && _isFloatTasksCompleted && _isStringTasksCompleted) {
				EventCenter.Instance.UpdateSceneTasksCompleted ();
			}
		}
		#endregion
	}

}

