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
		private LevelTaskData _tasks;

		private bool _isIntTasksCompleted = false; 
		private bool _isFloatTasksCompleted = false; 
		private bool _isStringTasksCompleted = false; 
		private bool _isCollectionTasksCompleted = false; 

		private Hashtable _gameDataTasks; 
		#endregion

		#region handlers
		public void OnIntTaskUpdated(string name, int value) {
			IntTaskData task = _findTask (_tasks.intTasks, name) as IntTaskData;
			if (task != null) {
				task.current = value;
				if (task.current == task.goal) {
					_taskCompleted (task, _tasks.intTasks, out _isIntTasksCompleted);
				}
			}
		}

		public void OnFloatTaskUpdated(string name, float value) {
			FloatTaskData task = _findTask (_tasks.floatTasks, name) as FloatTaskData;
			task.current = value;
			if (task.current == task.goal) {
				_taskCompleted (task, _tasks.floatTasks, out _isFloatTasksCompleted);
			}
		}

		public void OnStringTaskUpdated(string name, string value) {
			StringTaskData task = _findTask (_tasks.stringTasks, name) as StringTaskData;
			task.current = value;
			if (task.current == task.goal) {
				_taskCompleted (task, _tasks.stringTasks, out _isStringTasksCompleted);
			}
		}

		public void OnInventoryAdded(string item, int count, bool isPlayerInventory = false) {
			if (isPlayerInventory) {
				CollectionTaskData task = _findTask (_tasks.collectionTasks, item) as CollectionTaskData;
				if (task != null) {
					task.current = count;
					if (task.current == task.goal) {
						_taskCompleted (task, _tasks.collectionTasks, out _isCollectionTasksCompleted);
					}
				}
			}
		}
		#endregion

		#region public methods
		public void Init(LevelTaskData tasks) {
			// Debug.Log ("TaskController/Init, tasks = " + tasks);
			_tasks = tasks;

			if (_tasks.intTasks.Length == 0) {
				_isIntTasksCompleted = true;
			}
			if (_tasks.floatTasks.Length == 0) {
				_isFloatTasksCompleted = true;
			}
			if (_tasks.stringTasks.Length == 0) {
				_isStringTasksCompleted = true;
			}
			if (_tasks.collectionTasks.Length == 0) {
				_isCollectionTasksCompleted = true;
			}

			EventCenter ec = EventCenter.Instance;
			ec.OnIntTaskUpdated += OnIntTaskUpdated;
			ec.OnFloatTaskUpdated += OnFloatTaskUpdated;
			ec.OnStringTaskUpdated += OnStringTaskUpdated;
			ec.OnInventoryAdded += OnInventoryAdded;
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
			// Debug.Log ("TaskController/_taskCompleted, task = " + task.name);
			task.isCompleted = true;

			isTypeCompleted = true;
			for (int i = 0; i < tasks.Length; i++) {
				if (!tasks [i].isCompleted) {
					isTypeCompleted = false;
					break;
				}
			}
			// Debug.Log (" isTypeCompleted = " + isTypeCompleted);
			if (isTypeCompleted && _isIntTasksCompleted && _isFloatTasksCompleted && _isStringTasksCompleted) {
				// Debug.Log ("TaskController/_allTasksCompletedCheck");
				EventCenter.Instance.UpdateLevelTasksCompleted ();
			}
		}

		private void OnDestroy() {
			EventCenter ec = EventCenter.Instance;
			if (ec != null) {
				ec.OnIntTaskUpdated -= OnIntTaskUpdated;
				ec.OnFloatTaskUpdated -= OnFloatTaskUpdated;
				ec.OnStringTaskUpdated -= OnStringTaskUpdated;
				ec.OnInventoryAdded -= OnInventoryAdded;
			}
		}
		#endregion
	}

}

