using UnityEngine;
using System.Collections;

public class InspectableItem : InteractiveItem {

	public string description = "";

	void Awake() {
		InitInteractiveItem();
	}

	public void InitInteractiveItem() {
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
