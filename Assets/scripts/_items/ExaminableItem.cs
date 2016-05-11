using UnityEngine;
using System.Collections;

public class ExaminableItem : InteractiveItem {

	public string description = "";
	public bool singleUse = true;

	private bool _isUsedOnce = false;

	void Awake() {
		InitInteractiveItem();
	}

	public void InitInteractiveItem() {
		Init();
	}
	
	public override void Actuate() {
		if(IsEnabled) {
			if(CheckProximity()) {
				if(!singleUse || !_isUsedOnce) {
					_isUsedOnce = true;
					EventCenter.Instance.AddNote(description);
				}
			}
		}
	}
}
