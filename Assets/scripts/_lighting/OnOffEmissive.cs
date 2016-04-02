using UnityEngine;
using System.Collections;

public class OnOffEmissive : Toggler {

	public float onEmission = 1f;
	public float offEmission = 0f;

	private Material _material;

	public override void Toggle() {
		isOn = !isOn;
		ToggleTarget (isOn);
	}

	public override void ToggleTarget(bool turnOn) {
		isOn = turnOn;
//		Debug.Log ("ToggleTarget[" + this.name + "], isOn = " + isOn);
		float emission = (isOn) ? onEmission : offEmission;
		Color baseColor = _material.GetColor("_EmissionColor");

		Color finalColor = baseColor * Mathf.LinearToGammaSpace (emission);
//		Debug.Log ("finalColor = " + finalColor);
		_material.SetColor ("_EmissionColor", finalColor);
	}

	void Awake() {
		Renderer _renderer = GetComponent<Renderer> ();
		_material = _renderer.material;
		ToggleTarget (isOn);
	}
	
}
