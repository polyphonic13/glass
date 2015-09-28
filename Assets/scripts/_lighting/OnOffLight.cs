using UnityEngine;

public class OnOffLight : InteractiveItem {

	public Light bulb; 
	public bool enabled = false;

	void Awake() {
		InitOnOffLight();
		Init();
	}
	
	public void InitOnOffLight() {
		bulb.enabled = enabled;
	}

	public void OnActuate() {
		Actuate();
	}

	public override void Actuate() {
		if(IsRoomActive) {
			if(CheckProximity()) {
				Toggle();
			}
		}
	}

	public virtual void Toggle() {
		ToggleBulb(bulb);
	}
	
	public void ToggleBulb(Light light) {
		light.enabled = !light.enabled;
	}
	
	public bool GetIsOn() {
		return bulb.enabled;
	}
}
