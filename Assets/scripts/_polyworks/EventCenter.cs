using UnityEngine;
using System.Collections.Generic;

namespace Polyworks
{
	public class EventCenter: MonoBehaviour
	{
		public delegate void InventoryAdder(string item);
		public event InventoryAdder OnInventoryAdded;

		public delegate void NoteAdder(string message);
		public event NoteAdder OnNoteAdded;

		public delegate void SceneChanger(string scene);
		public event SceneChanger OnChangeScene;

		public delegate void CountTaskUpdater(string task, int count);
		public event CountTaskUpdater OnCountTaskUpdated; 
		
		public delegate void ValueTaskUpdater(string task, float value);
		public event ValueTaskUpdater OnValueTaskUpdated;
		
		public delegate void GoalTaskUpdater(string task, string goal);
		public event GoalTaskUpdater OnGoalTaskUpdated;
			
		#region singleton
		private static EventCenter _instance;
		private EventCenter() {}

		public static EventCenter Instance {
			get {
				if(_instance == null) {
					_instance = GameObject.FindObjectOfType(typeof(EventCenter)) as EventCenter;      
				}
				return _instance;
			}
		}
		#endregion

		public void AddInventory(string item) {
			if (OnInventoryAdded != null) {
				OnInventoryAdded (item);
			}
		}

		public void AddNote(string message) {
			if (OnNoteAdded != null) {
				OnNoteAdded (message);
			}
		}

		public void ChangeScene(string scene) {
			Debug.Log ("EventCenter/ChangeScene, scene = " + scene + ", OnChangeScene = " + OnChangeScene);
			if (OnChangeScene != null) {
				OnChangeScene (scene);
			}
		}

		public void UpdateCountTask(string task, int count) {
			if(OnCountTaskUpdated != null) {
				OnCountTaskUpdated(task, count);
			}
		}

		public void UpdateValueTask(string task, float value) {
			if(OnValueTaskUpdated != null) {
				OnValueTaskUpdated(task, value);
			}
		}

		public void UpdateGoalTask(string task, string goal) {
			if(OnGoalTaskUpdated != null) {
				OnGoalTaskUpdated(task, goal);
			}
		}
	}
}

