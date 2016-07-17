using UnityEngine;

public class OnOffLight : Toggler {

	public Light light; 

	public override void Toggle() {
		isOn = !isOn;
		ToggleTarget(isOn);
	}
	
	public override void ToggleTarget(bool turnOn) {
		isOn = turnOn;
//		// Debug.Log ("ToggleTarget[" + this.name + "], isOn = " + isOn);
		light.enabled = isOn;
	}

	void Awake() {
		ToggleTarget (isOn);
	}
}
