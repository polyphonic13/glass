using UnityEngine;
using System.Collections.Generic;

namespace Polyworks
{
	public class EventCenter: MonoBehaviour
	{
		#region delegates
		public delegate void ItemProximityHandler(Item item, bool isNear); 
		public event ItemProximityHandler OnNearItem;
		
		public delegate void InventoryAdder(string item, int count);
		public event InventoryAdder OnInventoryAdded;

		public delegate void InventoryRemover(string item, int count);
		public event InventoryRemover OnInventoryRemoved;

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

		public delegate void InspectItemHandler(bool isInspecting, string item);
		public event InspectItemHandler OnInspectItem; 

		public delegate void CloseMenuHandler();
		public event CloseMenuHandler OnCloseMenuUI;

		public delegate void CloseInventoryHandler();
		public event CloseInventoryHandler OnCloseInventoryUI;

		public delegate void FlashlightActuateHandler(); 
		public event FlashlightActuateHandler OnActuateFlashlight;
		#endregion
		
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

		#region handlers
		public void ChangeItemProximity(Item item, bool isNear) {
			if(OnNearItem != null) {
				OnNearItem(item, isNear);
			}
		}
		
		public void InventoryAdded(string item, int count) {
//			Debug.Log ("EventCenter/InventoryAdded, item = " + item + ", count = " + count);
			if (OnInventoryAdded != null) {
				OnInventoryAdded (item, count);
			}
		}

		public void InventoryRemoved(string item, int count) {
			if(OnInventoryRemoved != null) {
				OnInventoryRemoved(item, count);
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

		public void UpdateIntTask(string task, int value) {
			if(OnIntTaskUpdated != null) {
				OnIntTaskUpdated(task, value);
			}
		}

		public void UpdateFloatTask(string task, float value) {
			if(OnFloatTaskUpdated != null) {
				OnFloatTaskUpdated(task, value);
			}
		}

		public void UpdateStringTask(string task, string value) {
			if(OnStringTaskUpdated != null) {
				OnStringTaskUpdated(task, value);
			}
		}

		public void UpdateLevelTasksCompleted() {
			if (OnLevelTasksCompleted != null) {
				OnLevelTasksCompleted ();
			}
		}

		public void InspectItem(bool isInspecting, string item) {
			if (OnInspectItem != null) {
				OnInspectItem (isInspecting, item);
			}
		}

		public void CloseMenuUI() {
			if(OnCloseMenuUI != null) {
				OnCloseMenuUI();
			}
		}

		public void CloseInventoryUI() {
			if(OnCloseInventoryUI != null) {
				OnCloseInventoryUI();
			}
		}
		
		public void ActuateFlashlight() {
			if(OnActuateFlashlight != null) {
				OnActuateFlashlight();
			}
		}
		#endregion
	}
}

