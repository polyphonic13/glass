using UnityEngine;

public class EventCenter : MonoBehaviour {

	public delegate void PlayerDamageHandler(float damage); 

	public delegate void OnWaterHandler(bool water, Transform tgt);
	public delegate void UnderWaterHandler(bool under);

	public delegate void PlayerPropertyUpdater(string prop, float val);

	public delegate void RoomHandler(string room);
    
	public delegate void PlayerHandler(bool enable);
	public delegate void PlayerBreadcrumbHandler(Vector3 position);
	public delegate void MouseSensitivityHandler(float sensitivity);

	public delegate void CameraZoomHandler(bool zoom);

	public delegate void ActuateHandler(string name); 

	public delegate void InventoryAdder(string item);
	public delegate void InventoryRemover(string item);
	public delegate void InventoryIncrementer(string item);
	
	public delegate void TriggerEventHandler(string evt);
	public delegate void TriggerCollectedEventHandler(string evt);

	public delegate void InteractiveItemProximityHandler(InteractiveItem element, bool inProximity);

	public delegate void AddNoteHandler(string message = "", bool fadeOut = true);
	public delegate void RemoveNoteHandler(string message = "");

	public delegate void InspectItemHandler(bool isInspecting, string item);
	public delegate void UseItemHandler(string item);

	public delegate void CloseMenuHandler();
	public delegate void CloseInventoryHandler();

	public delegate void DayNightHandler (string state);

	public event PlayerDamageHandler OnPlayerDamaged;

	public event OnWaterHandler OnAboveWater; 
	public event UnderWaterHandler OnUnderWater; 

	public event PlayerPropertyUpdater OnPlayerPropertyUpdated;

	public event RoomHandler OnRoomEntered;
	public event RoomHandler OnRoomExited;

	public event PlayerHandler OnEnablePlayer; 
	public event PlayerBreadcrumbHandler OnPlayerBreadcrumb;
	public event MouseSensitivityHandler OnMouseSensitivityChange;
	public event CameraZoomHandler OnCameraZoom;

	public event ActuateHandler OnActuate;

	public event InventoryAdder OnInventoryAdded;
	public event InventoryRemover OnInventoryRemoved;
	public event InventoryIncrementer OnInventoryIncremented;
	
	public event TriggerEventHandler OnTriggerEvent;
	public event TriggerCollectedEventHandler OnTriggerCollectedEvent; 
	
	public event InteractiveItemProximityHandler OnNearInteractiveItem;

	public event AddNoteHandler OnAddNote; 
	public event RemoveNoteHandler OnRemoveNote; 

	public event InspectItemHandler OnInspectItem; 
	public event UseItemHandler OnUseItem; 

	public event CloseMenuHandler OnCloseMenuUI;
	public event CloseInventoryHandler OnCloseInventoryUI;

	public event DayNightHandler OnDayNightChange;

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

	public void DamagePlayer(float damage) {
		if(OnPlayerDamaged != null) {
			OnPlayerDamaged(damage);
		}
	}

	public void ChangeAboveWater(bool water, Transform tgt) {
		if(OnAboveWater != null) {
			OnAboveWater(water, tgt); 
		}
	}

	public void ChangeUnderWater(bool under) {
		if(OnUnderWater != null) {
			OnUnderWater(under);
		}
	}

	public void UpdatePlayerProperty(string prop, float val) {
//		Debug.Log("EventCenter/UpdatePlayerProperty, prop = " + prop + ", val = " + val);
		if(OnPlayerPropertyUpdated != null) {
			OnPlayerPropertyUpdated(prop, val);
		}
	}

	public void EnterRoom(string room) {
		if(OnRoomEntered != null) {
			OnRoomEntered(room);
		}
	}

	public void ExitRoom(string room) {
		if(OnRoomExited != null) {
			OnRoomExited(room);
		}
	}

	public void EnablePlayer(bool enable) {
		if(OnEnablePlayer != null) {
			OnEnablePlayer(enable);
		}
	}

	public void DropBreadcrumb(Vector3 position) {
		if(OnPlayerBreadcrumb != null) {
			OnPlayerBreadcrumb(position);
		}
	}

	public void ChangeMouseSensitivity(float sensitivity) {
//		Debug.Log("EventCenter/ChangeMouseSensitivity, sensitivity = " + sensitivity + ", OnMouseSensitivityChange = " + OnMouseSensitivityChange);
		if(OnMouseSensitivityChange != null) {
			OnMouseSensitivityChange(sensitivity);
		}
	}
	
	public void ZoomCamera(bool zoom) {
		if(OnCameraZoom != null) {
			OnCameraZoom(zoom);
		}
	}

	public void AddInventory(string item) {
		if(OnInventoryAdded != null) {
			OnInventoryAdded(item);
		}
	}

	public void RemoveInventory(string item) {
		if(OnInventoryRemoved != null) {
			OnInventoryRemoved(item);
		}
	}

	public void IncrementInventory(string item) {
		if(OnInventoryIncremented != null) {
			OnInventoryIncremented(item);
		}
	}
	
	public void TriggerEvent(string evt) {
		if(OnTriggerEvent != null) {
			OnTriggerEvent(evt);	
		}
	}
	
	public void TriggerCollectedEvent(string evt) {
		if(OnTriggerCollectedEvent != null) {
			OnTriggerCollectedEvent(evt);
		}
	}

	public void Actuate(string name) {
//		Debug.Log("EventCenter.Actuate, name = " + name);
		if(OnActuate != null) {
			OnActuate(name);
		}
	}
	
	public void NearInteractiveItem(InteractiveItem element, bool inProximity) {
		if(OnNearInteractiveItem != null) {
			OnNearInteractiveItem(element, inProximity);
		}
	}

	public void AddNote(string message = "", bool fadeOut = true) {
		if(OnAddNote != null) {
			OnAddNote(message, fadeOut);
		}
	}
	
	public void RemoveNote(string message = "") {
		if(OnRemoveNote != null) {
			OnRemoveNote(message);
		}
	}

	public void InspectItem(bool isInspecting, string item) {
		if (OnInspectItem != null) {
			OnInspectItem (isInspecting, item);
		}
	}

	public void UseItem(string item) {
		if (OnUseItem != null) {
			OnUseItem (item);
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

	public void ChangeDayNightState(string state) {
		Debug.Log ("ChangeDayNightState, state = " + state);
		if (OnDayNightChange != null) {
			OnDayNightChange (state);
		}
	}
}
