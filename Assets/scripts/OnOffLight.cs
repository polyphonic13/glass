using UnityEngine;
using System.Collections;

public class OnOffLight : InteractiveElement {

	public Light bulb; 
	public bool enabled = false;

	void Awake() {
		InitOnOffLight();
		Init(MouseManager.Instance.INTERACT_CURSOR);
	}
	
	public void InitOnOffLight() {
		bulb = transform.Search("light_bulb").light;
		bulb.enabled = enabled;
	}

	public override void OnHighlightStart() {
//		Debug.Log("OnOffLight[ " + name + " ]/OnHighlightStart, isRoomActive = " + isRoomActive + ", isEnabled = " + isEnabled);
		if(isRoomActive && isEnabled) {
			mouseOver();
		}
	}

	public void OnInputTaken() {
		InputTaken();
	}

	public override void InputTaken() {
		Debug.Log("OnOffLight[ " + name + " ]/InputTaken, isRoomActive = " + isRoomActive);
		if(isRoomActive) {
			var difference = Vector3.Distance(Camera.mainCamera.gameObject.transform.position, transform.position);
			//			Debug.Log("  difference = " + difference + ", bulb.enabled = " + bulb.enabled);
			if(difference < interactDistance) {
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
