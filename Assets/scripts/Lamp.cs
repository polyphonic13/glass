using UnityEngine;
using System.Collections;

public class Lamp : OnOffLight {
	
	private Lampshade _lampshade;
	
	void Awake() {
		InitLamp();
		Init(MouseManager.Instance.INTERACT_CURSOR);
	}

	void InitLamp() {
		_lampshade = GetComponentInChildren<Lampshade>();
		if(_lampshade != null) {
//			Debug.Log ("_lampshade = " + _lampshade.gameObject.transform.renderer.material.mainTexture);
			_lampshade.gameObject.transform.renderer.material = _lampshade.offMaterial;
		}
		InitOnOffLight();
	}
	
	public override void Toggle() {
		ToggleBulb(bulb);
		if(_lampshade != null) {
			if(GetIsOn()) {
				_lampshade.gameObject.transform.renderer.material = _lampshade.onMaterial;
			} else {
				_lampshade.gameObject.transform.renderer.material = _lampshade.offMaterial;
			}
		}
	}
}
