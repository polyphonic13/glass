using UnityEngine;

public class OnOffLight : InteractiveElement {

	public Light bulb; 
	public bool enabled = false;

	void Awake() {
		InitOnOffLight();
		Init(MouseManager.Instance.INTERACT_CURSOR);
	}
	
	public void InitOnOffLight() {
		bulb.enabled = enabled;
	}

	public override void OnHighlightStart() {
//		Debug.Log("OnOffLight[ " + name + " ]/OnHighlightStart, IsRoomActive = " + IsRoomActive + ", _isEnabled = " + _isEnabled);
		if(IsRoomActive && _isEnabled) {
			OnHighlightStart();
		}
	}

	public void OnInputTaken() {
		InputTaken();
	}

	public override void InputTaken() {
		Debug.Log("OnOffLight[ " + name + " ]/InputTaken, IsRoomActive = " + IsRoomActive);
		if(IsRoomActive) {
			var difference = Vector3.Distance(Camera.main.gameObject.transform.position, transform.position);
			//			Debug.Log("  difference = " + difference + ", bulb.enabled = " + bulb.enabled);
			if(difference < _interactDistance) {
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
