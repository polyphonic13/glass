using UnityEngine;

public class EventCenter : MonoBehaviour {

	public delegate void PlayerDamageHandler(float damage); 

	public delegate void OnWaterHandler(bool water, Transform tgt);
	public delegate void UnderWaterHandler(bool under);

	public delegate void PlayerPropertyUpdater(string prop, float val);

	public delegate void RoomHandler(string room);
    public delegate void NoteHandler(string msg = "", bool zoom = false);
	public delegate void PlayerHandler(bool enable);
	public delegate void PlayerBreadcrumbHandler(Vector3 position);
	public delegate void MouseSensitivityHandler(float sensitivity);

	public delegate void CameraZoomHandler(bool zoom);

	public delegate void InputTakenHandler(string name); 

	public delegate void InventoryAdder(string item);
	public delegate void InventoryRemover(string item);

	public delegate void EquipItemHandler(string itemName);
	
	public delegate void TriggerEventHandler(string evt);
	public delegate void TriggerCollectedEventHandler(string evt);

	public delegate void InteractiveElementProximityHandler(InteractiveElement element, bool inProximity);
	
	public event PlayerDamageHandler OnPlayerDamaged;

	public event OnWaterHandler OnAboveWater; 
	public event UnderWaterHandler OnUnderWater; 

	public event PlayerPropertyUpdater OnPlayerPropertyUpdated;

	public event RoomHandler OnRoomEntered;
	public event RoomHandler OnRoomExited;

    public event NoteHandler OnAddNote; 
	public event NoteHandler OnRemoveNote; 
	
	public event PlayerHandler OnEnablePlayer; 
	public event PlayerBreadcrumbHandler OnPlayerBreadcrumb;
	public event MouseSensitivityHandler OnMouseSensitivityChange;
	public event CameraZoomHandler OnCameraZoom;

	public event InputTakenHandler OnInputTaken;

	public event InventoryAdder OnInventoryAdded;
	public event InventoryRemover OnInventoryRemoved;

	public event EquipItemHandler OnEquipItem;
	
	public event TriggerEventHandler OnTriggerEvent;
	public event TriggerCollectedEventHandler OnTriggerCollectedEvent; 
	
	public event InteractiveElementProximityHandler OnNearInteractiveElement;
	
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

	public void AddNote(string msg, bool zoom = false) {
		if(OnAddNote != null) {
			OnAddNote(msg, zoom);
		}
	}
	
	public void RemoveNote(string msg = "", bool zoom = false) {
		if(OnRemoveNote != null) {
			OnRemoveNote(msg, zoom);
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

	public void EquipItem(string itemName) {
		if(OnEquipItem != null){
			OnEquipItem(itemName);
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

	public void InputTaken(string name) {
//		Debug.Log("EventCenter.InputTaken, name = " + name);
		if(OnInputTaken != null) {
			OnInputTaken(name);
		}
	}
	
	public void NearInteractiveElement(InteractiveElement element, bool inProximity) {
		if(OnNearInteractiveElement != null) {
			OnNearInteractiveElement(element, inProximity);
		}
	}
}