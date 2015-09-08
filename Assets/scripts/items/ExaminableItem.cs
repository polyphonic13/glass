using UnityEngine;
using System.Collections;

public class ExaminableItem : InteractiveItem {

	public string description = "";

	void Awake() {
		InitInteractiveItem();
	}

	public void InitInteractiveItem() {
		Init();
	}
	
	public override void Actuate() {
		if(IsRoomActive) {
			if(CheckProximity()) {
				EventCenter.Instance.AddNote(description);
			}
		}
	}
}
