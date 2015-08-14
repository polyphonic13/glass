using UnityEngine;

public class InteractiveElement : MonoBehaviour {

	public float _interactDistance = 4;
	public string _containingRoom; 
	
	private int _activeCursor;

	public bool IsRoomActive { get; set; } 
	public bool IsEnabled { get; set; }

	void Awake() {
		Init();
	}

	public void Init(int activeCursor = 1) {
		IsEnabled = true;

		_activeCursor = activeCursor;

		if(transform.tag == "persistentItem") {
			IsRoomActive = true;
		} else {
			IsRoomActive = false;
	
			var eventCenter = EventCenter.Instance;
			eventCenter.OnRoomEntered += OnRoomEntered;
			eventCenter.OnRoomExited += OnRoomExited;
		}
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
//			Debug.Log ("InteractiveElement[" + name + "]/OnInputTaken, name = " + name);
			if(name == name) {
				InputTaken();
			}
		}
	}

	public virtual void InputTaken() {

	}
}
