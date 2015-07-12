using UnityEngine;
using System.Collections;

public class InteractiveElement : MonoBehaviour {

	public float interactDistance = 4;
	public string containingRoom; 
	public bool isEnabled = true;
	public bool willHighlight = true; 
	
	private MouseManager _mouseManager;
	private int _activeCursor;

	public Transform playerHead;

	public bool isRoomActive { get; set; } 

	void Awake() {
		Init();
	}

	public void Init(int activeCursor = 1) {
//		Debug.Log("InteractiveElement[ " + name + " ]/init, activeCursor = " + activeCursor);
		playerHead = GameObject.Find("FirstPersonCharacter").gameObject.transform;
//		_mouseManager = MouseManager.Instance;
//		_activeCursor = activeCursor;

		if(transform.tag == "persistentItem") {
			isRoomActive = true;
		} else {
			isRoomActive = false;
	
			var eventCenter = EventCenter.Instance;
			eventCenter.OnRoomEntered += OnRoomEntered;
			eventCenter.OnRoomExited += OnRoomExited;

		}
//		EventCenter.Instance.OnInputTaken += OnInputTaken;
	}

	public virtual void OnRoomEntered(string room) {
		if(room == containingRoom) {
//			Debug.Log("InteractiveElement[ " + name + " ]/OnRoomEntered");
			isRoomActive = true;
		}
	}

	public void OnRoomExited(string room) {
		if(room == containingRoom) {
//			Debug.Log("InteractiveElement[ " + name + " ]/OnRoomExited");
			isRoomActive = false;
		}
	}

	public void OnInputTaken(string name) {
//		if(isRoomActive && isEnabled) {
//			Debug.Log ("InteractiveElement[" + name + "]/OnInputTaken, name = " + name);
			if(name == name) {
				Debug.Log (name + " was clicked");
				InputTaken ();
			}
//		}

	}
	public virtual void InputTaken() {

	}

	public virtual void OnMouseOver() {
//		Debug.Log("InteractiveElement[ " + name + " ]/OnMouseOver, isRoomActive = " + isRoomActive + ", isEnabled = " + isEnabled);
		if(isRoomActive && isEnabled) {
			mouseOver();
		}
	}

	public void mouseOver() {
//		Debug.Log("InteractiveElement[ " + name + " ]/OnMouseOver, isRoomActive = " + isRoomActive);
		var difference = Vector3.Distance(playerHead.position, transform.position);
		if(difference < interactDistance) {
			_mouseManager.setCursorType(_activeCursor);
			if(willHighlight) {
				addHighlight();
			}
		}
	}

	public void addHighlight() {
		
	}
	
	public virtual void OnMouseExit() {
		if(isRoomActive && isEnabled) {
			mouseExit();
		}
	}

	public void mouseExit() {
		_mouseManager.setCursorType(MouseManager.Instance.DEFAULT_CURSOR);
		if(willHighlight) {
			removeHighlight();
		}
	}
	
	public void removeHighlight() {
		
	}
}
