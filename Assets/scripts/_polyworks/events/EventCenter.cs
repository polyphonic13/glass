using UnityEngine;
using System.Collections.Generic;

namespace Polyworks
{
	public class EventCenter: Singleton<EventCenter>
	{
		#region delegates
		public delegate void SceneChangeHandler(string scene, int section = -1);
		public event SceneChangeHandler OnStartSceneChange;
		public event SceneChangeHandler OnContinueSceneChange; 
		public event SceneChangeHandler OnCompleteSceneChange; 

		public delegate void IntEventHandler(string name, int value = 0);
		public event IntEventHandler OnIntEvent;

		public delegate void StringEventHandler(string name, string value = "");
		public event StringEventHandler OnStringEvent;

		public delegate void DestroyEventHandler(string target); 
		public event DestroyEventHandler OnDestroyEvent;

		public delegate void LevelInitializedHandler();
		public event LevelInitializedHandler OnLevelInitialized; 

		public delegate void ItemProximityHandler(Item item, bool isNear); 
		public event ItemProximityHandler OnNearItem;
		
		public delegate void InventoryAdder(string item, int count, bool isPlayerInventory);
		public event InventoryAdder OnInventoryAdded;

		public delegate void InventoryRemover(string item, int count);
		public event InventoryRemover OnInventoryRemoved;

		public delegate void NoteAdder(string message);
		public event NoteAdder OnAddNote;

		public delegate void SceneInitializer(string scene);
		public event SceneInitializer OnSceneInitialized;

		public delegate void PrefabInitializer();
		public event PrefabInitializer OnPrefabsAdded;

		public delegate void MainCameraInitializer();
		public event MainCameraInitializer OnMainCameraEnabled;

		public delegate void SectionChanger(int section);
		public event SectionChanger OnSectionChanged; 

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

		public delegate void FlashlightCollectHandler(); 
		public event FlashlightCollectHandler OnCollectFlashlight;

		public delegate void FlashlightEnableHandler(); 
		public event FlashlightEnableHandler OnEnableFlashlight;
		#endregion

		#region handlers
		public void StartSceneChange(string scene, int section = -1) {
			if (OnStartSceneChange != null) {
				OnStartSceneChange (scene, section);
			}
		}

		public void ContinueSceneChange(string scene, int section = -1) {
			if (OnContinueSceneChange != null) {
				OnContinueSceneChange (scene, section);
			}
		}

		public void CompleteSceneChange(string scene, int section = -1) {
			if (OnCompleteSceneChange != null) {
				OnCompleteSceneChange (scene, section);
			}
		}

		public void InvokeIntEvent(string type, int value = 0) {
			if (OnIntEvent != null) {
				OnIntEvent (type, value);
			}
		}

		public void InvokeStringEvent(string type, string value = "") {
			if (OnStringEvent != null) {
				OnStringEvent (type, value);
			}
		}

		public void InvokeDestroy(string target) {
			if (OnDestroyEvent != null) {
				OnDestroyEvent (target);
			}
		}

		public void LevelInitialized() {
			if (OnLevelInitialized != null) {
				OnLevelInitialized ();
			}
		}

		public void ChangeItemProximity(Item item, bool isNear) {
			if(OnNearItem != null) {
				OnNearItem(item, isNear);
			}
		}
		
		public void InventoryAdded(string item, int count, bool isPlayerInventory = false) {
			// Debug.Log ("EventCenter/InventoryAdded, item = " + item + ", count = " + count);
			if (OnInventoryAdded != null) {
				OnInventoryAdded (item, count, isPlayerInventory);
			}
		}

		public void InventoryRemoved(string item, int count) {
			if(OnInventoryRemoved != null) {
				OnInventoryRemoved(item, count);
			}
		}

		public void AddNote(string message) {
			if (OnAddNote != null) {
				OnAddNote (message);
			}
		}

		public void SceneInitializationComplete(string scene) {
			if (OnSceneInitialized != null) {
				OnSceneInitialized (scene);
			}
		}

		public void PrefabsAdded() {
			if (OnPrefabsAdded != null) {
				OnPrefabsAdded ();
			}
		}

		public void MainCameraEnabled() {
			if (OnMainCameraEnabled != null) {
				OnMainCameraEnabled ();
			}
		}

		public void ChangeSection(int section) {
			if (OnSectionChanged != null) {
				OnSectionChanged (section);
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

		public void CollectFlashight() {
			if (OnCollectFlashlight != null) {
				OnCollectFlashlight ();
			}
		}

		public void EnableFlashlight() {
			if(OnEnableFlashlight != null) {
				OnEnableFlashlight();
			}
		}
		#endregion
	}
}

