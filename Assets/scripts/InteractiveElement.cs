using UnityEngine;

public class InteractiveElement : MonoBehaviour {

	public float _interactDistance = 4;
	public string _containingRoom; 
	public bool _isEnabled = true;
	public bool _willHighlight = true; 
	
	private MouseManager _mouseManager;
	private int _activeCursor;

	public Transform _playerHead;

	public bool IsRoomActive { get; set; } 

	void Awake() {
		Init();
	}

	public void Init(int activeCursor = 1) {
//		Debug.Log("InteractiveElement[ " + name + " ]/init, activeCursor = " + activeCursor);
		_playerHead = GameObject.Find("FirstPersonCharacter").gameObject.transform;
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

	public void OnInputTaken(string n) {
//		if(IsRoomActive && _isEnabled) {
//			Debug.Log ("InteractiveElement[" + name + "]/OnInputTaken, name = " + name);
			if(n == name) {
				Debug.Log (name + " was clicked");
				InputTaken ();
			}
//		}

	}
	public virtual void InputTaken() {

	}

	public virtual void OnMouseOver() {
//		Debug.Log("InteractiveElement[ " + name + " ]/OnMouseOver, IsRoomActive = " + IsRoomActive + ", _isEnabled = " + _isEnabled);
		if(IsRoomActive && _isEnabled) {
			MouseOver();
		}
	}

	public void MouseOver() {
//		Debug.Log("InteractiveElement[ " + name + " ]/OnMouseOver, IsRoomActive = " + IsRoomActive);
		var difference = Vector3.Distance(_playerHead.position, transform.position);
		if(difference < _interactDistance) {
			_mouseManager.setCursorType(_activeCursor);
			if(_willHighlight) {
				AddHighlight();
			}
		}
	}

	public void AddHighlight() {
		
	}
	
	public virtual void OnMouseExit() {
		if(IsRoomActive && _isEnabled) {
			MouseExit();
		}
	}

	public void MouseExit() {
		_mouseManager.setCursorType(MouseManager.Instance.DEFAULT_CURSOR);
		if(_willHighlight) {
			RemoveHighlight();
		}
	}
	
	public void RemoveHighlight() {
		
	}
}
