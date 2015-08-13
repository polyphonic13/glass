using UnityEngine;

public class InteractiveElement : MonoBehaviour {

	public float _interactDistance = 4;
	public string _containingRoom; 
	public bool _isEnabled = true;
	
	// private MouseManager _mouseManager;
	private int _activeCursor;

	public Transform PlayerHead { get; set; }
	public bool IsRoomActive { get; set; } 

	void Awake() {
		Init();
	}

	public void Init(int activeCursor = 1) {
//		Debug.Log("InteractiveElement[ " + name + " ]/Init, activeCursor = " + activeCursor);
		PlayerHead = GameObject.Find("FirstPersonCharacter").gameObject.transform;
//		_mouseManager = MouseManager.Instance;
		_activeCursor = activeCursor;

		if(transform.tag == "persistentItem") {
			IsRoomActive = true;
		} else {
			IsRoomActive = false;
	
			var eventCenter = EventCenter.Instance;
			eventCenter.OnRoomEntered += OnRoomEntered;
			eventCenter.OnRoomExited += OnRoomExited;

		}
//		EventCenter.Instance.OnInputTaken += OnInputTaken;
	}

	public virtual void OnRoomEntered(string room) {
		if(room == _containingRoom) {
//			Debug.Log("InteractiveElement[ " + name + " ]/OnRoomEntered");
			IsRoomActive = true;
		}
	}

	public void OnRoomExited(string room) {
		if(room == _containingRoom) {
//			Debug.Log("InteractiveElement[ " + name + " ]/OnRoomExited");
			IsRoomActive = false;
		}
	}

	public void OnInputTaken(string name) {
//		if(IsRoomActive && _isEnabled) {
//			Debug.Log ("InteractiveElement[" + name + "]/OnInputTaken, name = " + name);
			if(name == name) {
				Debug.Log (name + " was clicked");
				InputTaken();
			}
//		}

	}
	public virtual void InputTaken() {

	}

}
