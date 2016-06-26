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

		public delegate void SceneInitializer(string scene);
		public event SceneInitializer OnSceneInitialized;

		public delegate void IntTaskUpdater(string task, int count);
		public event IntTaskUpdater OnIntTaskUpdated; 
		
		public delegate void FloatTaskUpdater(string task, float value);
		public event FloatTaskUpdater OnFloatTaskUpdated;
		
		public delegate void StringTaskUpdater(string task, string goal);
		public event StringTaskUpdater OnStringTaskUpdated;
			
		public delegate void LevelTasksCompleteNotifier ();
		public event LevelTasksCompleteNotifier OnLevelTasksCompleted;

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
			// Debug.Log ("EventCenter/ChangeScene, scene = " + scene + ", OnChangeScene = " + OnChangeScene);
			if (OnChangeScene != null) {
				OnChangeScene (scene);
			}
		}

		public void SceneInitializationComplete(string scene) {
			if (OnSceneInitialized != null) {
				OnSceneInitialized (scene);
			}
		}

		public void UpdateIntTask(string task, int count) {
			if(OnIntTaskUpdated != null) {
				OnIntTaskUpdated(task, count);
			}
		}

		public void UpdateFloatTask(string task, float value) {
			if(OnFloatTaskUpdated != null) {
				OnFloatTaskUpdated(task, value);
			}
		}

		public void UpdateStringTask(string task, string goal) {
			if(OnStringTaskUpdated != null) {
				OnStringTaskUpdated(task, goal);
			}
		}

		public void UpdateLevelTasksCompleted() {
			if (OnLevelTasksCompleted != null) {
				OnLevelTasksCompleted ();
			}
		}
	}
}

