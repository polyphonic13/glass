using UnityEngine;

public class InteractiveElement : MonoBehaviour {

	public Sprite thumbnail;
	public string itemName;

	public float _interactDistance = 3f;
	public string _containingRoom; 
	
	public bool IsRoomActive { get; set; } 
	public bool IsEnabled { get; set; }

	protected Camera mainCamera;

	void Awake() {
		Init();
	}

	public void Init() {
		IsEnabled = true;

		mainCamera = Camera.main;

		if(transform.tag == "persistentItem" || _containingRoom == null) {
			IsRoomActive = true;
		} else {
			IsRoomActive = false;
	
			var eventCenter = EventCenter.Instance;
			eventCenter.OnRoomEntered += OnRoomEntered;
			eventCenter.OnRoomExited += OnRoomExited;
		}
	}

	public string getName() {
		return itemName;
	}

	public Camera getCamera() {
		return mainCamera;
	}

	public virtual void OnRoomEntered(string room) {
		if(room == _containingRoom) {
			IsRoomActive = true;
		}
	}

	public void OnRoomExited(string room) {
		if(room == _containingRoom) {
			IsRoomActive = false;
		}
	}

	public void OnInputTaken(string name) {
		if(IsRoomActive && IsEnabled) {
			if(name == itemName) {
				InputTaken();
			}
		}
	}

	public virtual void InputTaken() {}

	public bool CheckProximity() {
		var isInProximity = false;
		var difference = Vector3.Distance(mainCamera.transform.position, transform.position);
		if(difference < _interactDistance) {
			isInProximity = true;
		}
		return isInProximity;
	}
}
