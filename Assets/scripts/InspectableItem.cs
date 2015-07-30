using UnityEngine;
using System.Collections;

public class InspectableItem : InteractiveElement {

	public string description = "";

	void Awake() {
//		Debug.Log("InspectableItem[ " + name + " ]/Awake");
		InitInteractiveElement();
	}

	public void InitInteractiveElement() {
		Init(MouseManager.Instance.MAGNIFY_CURSOR);
//		Init(3);
	}
	
	public void OnInputTaken() {
		InputTaken();
	}

	public override void InputTaken() {
		var difference = Vector3.Distance(Camera.main.gameObject.transform.position, transform.position);
		if(difference < _interactDistance) {
//			Debug.Log("InspectableItem/OnInputTaken, IsRoomActive = " + IsRoomActive);
			if(IsRoomActive) {
				EventCenter.Instance.AddNote(description);
			}
		}
	}
}
