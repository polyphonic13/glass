using UnityEngine;
using System.Collections;

public class Lamp : OnOffLight {
	
	private Lampshade _lampshade;
	
	void Awake() {
		InitLamp();
		Init();
	}

	void InitLamp() {
		_lampshade = GetComponentInChildren<Lampshade>();
		if(_lampshade != null) {
			_lampshade.gameObject.transform.GetComponent<Renderer>().material = _lampshade.offMaterial;
		}
		InitOnOffLight();
	}
	
	public override void Toggle() {
		ToggleBulb(bulb);
		if(_lampshade != null) {
			if(GetIsOn()) {
				_lampshade.gameObject.transform.GetComponent<Renderer>().material = _lampshade.onMaterial;
			} else {
				_lampshade.gameObject.transform.GetComponent<Renderer>().material = _lampshade.offMaterial;
			}
		}
	}
}
