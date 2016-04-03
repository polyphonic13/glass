using UnityEngine;
using System.Collections;

public class OnOffEmissive : Toggler {

	public float onEmission = 1f;
	public float offEmission = 0f;

	private Material _material;
	private Color _onColor;
	private Color _offColor = Color.black;

	public override void Toggle() {
		isOn = !isOn;
		ToggleTarget (isOn);
	}

	public override void ToggleTarget(bool turnOn) {
		isOn = turnOn;
//		Debug.Log ("ToggleTarget[" + this.name + "], isOn = " + isOn);
		float emission = (isOn) ? onEmission : offEmission;
		Color baseColor = (isOn) ? _onColor : _offColor;

		Color finalColor = baseColor * Mathf.LinearToGammaSpace (emission);
		Debug.Log ("finalColor = " + finalColor + ", emission = " + emission + ", baseColor = " + _onColor);
		_material.SetColor ("_EmissionColor", finalColor);
	}

	void Awake() {
		Renderer _renderer = GetComponent<Renderer> ();
		_material = _renderer.material;
		_onColor = _material.GetColor ("_EmissionColor");
//		Debug.Log ("emission color = " + _onColor);
		ToggleTarget (isOn);
	}
	
}
