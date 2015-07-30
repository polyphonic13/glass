using UnityEngine;
using System.Collections;

public class Lamp : OnOffLight {
	
	private Lampshade _lampshade;
	
	void Awake() {
		initLamp();
		init(MouseManager.Instance.INTERACT_CURSOR);
	}

	void initLamp() {
		_lampshade = GetComponentInChildren<Lampshade>();
		if(_lampshade != null) {
//			Debug.Log ("_lampshade = " + _lampshade.gameObject.transform.renderer.material.mainTexture);
			_lampshade.gameObject.transform.renderer.material = _lampshade.offMaterial;
		}
		initOnOffLight();
	}
	
	public override void toggle() {
		this.toggleBulb(this.bulb);
		if(_lampshade != null) {
			if(this.getIsOn()) {
				_lampshade.gameObject.transform.renderer.material = _lampshade.onMaterial;
			} else {
				_lampshade.gameObject.transform.renderer.material = _lampshade.offMaterial;
			}
		}
	}
}
