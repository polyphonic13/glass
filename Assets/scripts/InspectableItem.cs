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
		if(CheckProximity()) {
			if(IsRoomActive) {
				EventCenter.Instance.AddNote(description);
			}
		}
	}
}
