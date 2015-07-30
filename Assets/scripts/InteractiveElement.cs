using UnityEngine;

public class InteractiveElement : MonoBehaviour {

	public float _interactDistance = 4;
	public string _containingRoom; 
	public bool _isEnabled = true;
	public bool _willHighlight = true; 
	
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

	public virtual void OnHighlightStart() {
//		Debug.Log("InteractiveElement[ " + name + " ]/OnHighlightStart, IsRoomActive = " + IsRoomActive + ", _isEnabled = " + _isEnabled);
		if(IsRoomActive && _isEnabled) {
			Highlight();
		}
	}

	public void Highlight() {
//		Debug.Log("InteractiveElement[ " + name + " ]/OnHighlightStart, IsRoomActive = " + IsRoomActive);
		var difference = Vector3.Distance(PlayerHead.position, transform.position);
		if(difference < _interactDistance) {
			// _mouseManager.setCursorType(_activeCursor);
			if(_willHighlight) {
				AddHighlight();
			}
		}
	}

	public void AddHighlight() {
		
	}
	
	public virtual void OnHighlightEnd() {
		if(IsRoomActive && _isEnabled) {
			HighlightEnd();
		}
	}

	public void HighlightEnd() {
		// _mouseManager.setCursorType(MouseManager.Instance.DEFAULT_CURSOR);
		if(_willHighlight) {
			RemoveHighlight();
		}
	}
	
	public void RemoveHighlight() {
		Debug.Log("remove highlight");
	}
}
