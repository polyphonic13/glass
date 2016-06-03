using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Polyworks
{
	[Serializable]
	public class TaskController: MonoBehaviour
	{
		[SerializeField] public TaskCollection<int> countTasks;
		[SerializeField] public TaskCollection<float> valueTasks;
		[SerializeField] public TaskCollection<string> goalTasks;

		private GameData _gameData;

		public void Init(Hashtable completedTasks) {
			countTasks.InitCompleted (completedTasks);
			valueTasks.InitCompleted (completedTasks);
			goalTasks.InitCompleted (completedTasks);
		}

		#region handlers
		public void OnCountTaskUpdated(string name, int value) {
			Task<int> task = countTasks.Find (name) as Task<int>;

			if (task != null) {
				task.SetValue (value);

				if (task.isComplete) {
					_gameData.completedTasks.Add (task.name, true);
				}
			}
		}

		public void OnValueTaskUpdated(string name, float value) {
			Task<float> task = valueTasks.Find (name) as Task<float>;

			if (task != null) {
				task.SetValue (value);

				if (task.isComplete) {
					_gameData.completedTasks.Add (task.name, true);
				}
			}
		}
		public void OnCountTaskUpdated(string name, string value) {
			Task<string> task = goalTasks.Find (name) as Task<string>;

			if (task != null) {
				task.SetValue (value);

				if (task.isComplete) {
					_gameData.completedTasks.Add (task.name, true);
				}
			}
		}
		#endregion

	}

	[Serializable]
	public class TaskCollection<T> where T: IComparable{
		public Task<T>[] tasks;

		public bool isAllComplete = false;

		private int _completed = 0; 

		public Task<T> Find(string name) {
			for (int i = 0; i < tasks.Length; i++) {
				if (tasks [i].name == name) {
					return tasks [i];
				}
			}
			return null;
		}

		public void InitCompleted(Hashtable completedTasks) {
			for (int i = 0; i < tasks.Length; i++) {
				if (completedTasks.Contains (tasks [i].name)) {
					tasks [i].isComplete = true;
					_completed++;
				}
			}
			if (_completed == tasks.Length) {
				isAllComplete = true;
			}
		}
	}

	[Serializable]
	public class Task<T> where T: IComparable{
		public string name;
		public bool isComplete = false;

		public T value;
		public T goal;

		public void SetValue(T val) {
			value = val;
			if (value.CompareTo (goal) == 0) {
				isComplete = true;
			}
		}
	}
}

