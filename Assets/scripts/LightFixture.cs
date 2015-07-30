using UnityEngine;
using System.Collections;

public class LightFixture : OnOffLight {
	
	private Component[] _onOffSelfIllums;
	private ArrayList _bulbs;
	private bool isOn = true;
	
	void Awake() {
		InitLightFixture();
		Init(MouseManager.Instance.INTERACT_CURSOR);
	}

	void InitLightFixture() {
		_onOffSelfIllums = GetComponentsInChildren<OnOffSelfIllum>();
		if(_onOffSelfIllums != null) {
			_toggleSelfIllums(true);
		}
		_bulbs = new ArrayList();
		
		Transform[] transforms = gameObject.GetComponentsInChildren<Transform>();
		foreach(Transform transform in transforms) {
			if(transform.name == "light_bulb") {
				_bulbs.Add(transform);
			}
		}
		Debug.Log("LightFixture, _bulbs.length = " + _bulbs);
		ToggleBulbs();
//		InitOnOffLight();
	}
	
	public override void Toggle() {
		Debug.Log("LightFixure/Toggle, isOn = " + isOn);
		if(_onOffSelfIllums != null) {
			if(isOn) {
				_toggleSelfIllums(false);
			} else {
				_toggleSelfIllums(true);
			}
		}
		isOn = !isOn;
		ToggleBulbs();
	}
	
	public void ToggleBulbs() {
		Debug.Log("LightFixture/ToggleBulbs");
		foreach(Transform bulb in _bulbs) {
			ToggleBulb(bulb.GetComponent<Light>());
		}
	} 
	
	private void _toggleSelfIllums(bool turnOff) {
		foreach(OnOffSelfIllum onOffSelfIllum in _onOffSelfIllums) {
			if(turnOff) {
				_turnOffSelfIllum(onOffSelfIllum);
			} else {
				_turnOnSelfIllum(onOffSelfIllum);
			}
		}
	}
	
	private void _turnOffSelfIllum(OnOffSelfIllum onOffSelfIllum) {
		onOffSelfIllum.gameObject.transform.GetComponent<Renderer>().material = onOffSelfIllum.offMaterial;
	}

	private void _turnOnSelfIllum(OnOffSelfIllum onOffSelfIllum) {
		onOffSelfIllum.gameObject.transform.GetComponent<Renderer>().material = onOffSelfIllum.onMaterial;
	}
}
