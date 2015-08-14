using UnityEngine;
using System.Collections;

public class InspectableItem : InteractiveElement {

	public string description = "";

	void Awake() {
		InitInteractiveElement();
	}

	public void InitInteractiveElement() {
		Init();
	}
	
	public void OnInputTaken() {
		InputTaken();
	}

	public override void InputTaken() {
		var difference = Vector3.Distance(Camera.main.gameObject.transform.position, transform.position);
		if(difference < _interactDistance) {
			if(IsRoomActive) {
				EventCenter.Instance.AddNote(description);
			}
		}
	}
}
