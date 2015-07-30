using UnityEngine;
using System.Collections;

public class InspectableItem : InteractiveElement {

	public string description = "";

	void Awake() {
//		Debug.Log("InspectableItem[ " + this.name + " ]/Awake");
		initInteractiveElement();
	}

	public void initInteractiveElement() {
		init(MouseManager.Instance.MAGNIFY_CURSOR);
//		init(3);
	}
	
	public void OnMouseDown() {
		this.mouseClick ();
	}

	public override void mouseClick() {
		var difference = Vector3.Distance(Camera.mainCamera.gameObject.transform.position, this.transform.position);
		if(difference < interactDistance) {
//			Debug.Log("InspectableItem/OnMouseDown, this.isRoomActive = " + this.isRoomActive);
			if(this.isRoomActive) {
				EventCenter.Instance.addNote(description);
			}
		}
	}
}
