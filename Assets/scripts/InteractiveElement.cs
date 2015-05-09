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
		init();
	}

	public void init(int activeCursor = 1) {
//		Debug.Log("InteractiveElement[ " + this.name + " ]/init, activeCursor = " + activeCursor);
		playerHead = GameObject.Find("FirstPersonCharacter").gameObject.transform;
//		_mouseManager = MouseManager.Instance;
//		_activeCursor = activeCursor;

		if(this.transform.tag == "persistentItem") {
			this.isRoomActive = true;
		} else {
			this.isRoomActive = false;
	
			var eventCenter = EventCenter.instance;
			eventCenter.onRoomEntered += this.onRoomEntered;
			eventCenter.onRoomExited += this.onRoomExited;

		}
//		EventCenter.instance.onMouseClick += this.OnMouseClick;
	}

	public virtual void onRoomEntered(string room) {
		if(room == this.containingRoom) {
//			Debug.Log("InteractiveElement[ " + this.name + " ]/onRoomEntered");
			this.isRoomActive = true;
		}
	}

	public void onRoomExited(string room) {
		if(room == this.containingRoom) {
//			Debug.Log("InteractiveElement[ " + this.name + " ]/onRoomExited");
			this.isRoomActive = false;
		}
	}

	public void OnMouseClick(string name) {
//		if(this.isRoomActive && this.isEnabled) {
//			Debug.Log ("InteractiveElement[" + this.name + "]/onmouseclick, name = " + name);
			if(this.name == name) {
				Debug.Log (this.name + " was clicked");
				this.mouseClick ();
			}
//		}

	}
	public virtual void mouseClick() {

	}

	public virtual void OnMouseOver() {
//		Debug.Log("InteractiveElement[ " + this.name + " ]/OnMouseOver, isRoomActive = " + this.isRoomActive + ", isEnabled = " + this.isEnabled);
		if(this.isRoomActive && this.isEnabled) {
			mouseOver();
		}
	}

	public void mouseOver() {
//		Debug.Log("InteractiveElement[ " + this.name + " ]/OnMouseOver, this.isRoomActive = " + this.isRoomActive);
		var difference = Vector3.Distance(playerHead.position, this.transform.position);
		if(difference < interactDistance) {
			_mouseManager.setCursorType(_activeCursor);
			if(this.willHighlight) {
				this.addHighlight();
			}
		}
	}

	public void addHighlight() {
		
	}
	
	public virtual void OnMouseExit() {
		if(this.isRoomActive && this.isEnabled) {
			mouseExit();
		}
	}

	public void mouseExit() {
		_mouseManager.setCursorType(MouseManager.Instance.DEFAULT_CURSOR);
		if(this.willHighlight) {
			this.removeHighlight();
		}
	}
	
	public void removeHighlight() {
		
	}
}
