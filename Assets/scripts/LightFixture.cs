using UnityEngine;
using System.Collections;

public class LightFixture : OnOffLight {
	
	private Component[] _onOffSelfIllums;
	private ArrayList _bulbs;
	private bool isOn = true;
	
	void Awake() {
		initLightFixture();
		init(MouseManager.Instance.INTERACT_CURSOR);
	}

	void initLightFixture() {
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
		this.toggleBulbs();
//		initOnOffLight();
	}
	
	public override void toggle() {
		Debug.Log("LightFixure/toggle, isOn = " + this.isOn);
		if(_onOffSelfIllums != null) {
			if(this.isOn) {
				_toggleSelfIllums(false);
			} else {
				_toggleSelfIllums(true);
			}
		}
		this.isOn = !this.isOn;
		this.toggleBulbs();
	}
	
	public void toggleBulbs() {
		Debug.Log("LightFixture/toggleBulbs");
		foreach(Transform bulb in _bulbs) {
			this.toggleBulb(bulb.light);
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
		onOffSelfIllum.gameObject.transform.renderer.material = onOffSelfIllum.offMaterial;
	}

	private void _turnOnSelfIllum(OnOffSelfIllum onOffSelfIllum) {
		onOffSelfIllum.gameObject.transform.renderer.material = onOffSelfIllum.onMaterial;
	}
}
