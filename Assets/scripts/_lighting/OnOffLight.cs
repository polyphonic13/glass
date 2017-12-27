using UnityEngine;
using Polyworks; 

public class OnOffLight : Toggler {

	public Light bulb; 

	public override void Toggle() {
		isOn = !isOn;
		ToggleTarget(isOn);
	}
	
	public override void ToggleTarget(bool turnOn) {
		isOn = turnOn;
		 Debug.Log ("ToggleTarget[" + this.name + "], isOn = " + isOn);
		bulb.enabled = isOn;
	}

	void Awake() {
		ToggleTarget (isOn);
	}
}
