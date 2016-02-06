using UnityEngine;
using Rewired;

public class Flashlight : MonoBehaviour {

	private Light _bulb;
	private Rewired.Player _controls;

	void Awake() {
		_bulb = gameObject.GetComponent<Light>();
		_bulb.enabled = false;
		_controls = ReInput.players.GetPlayer(0);
	}
	
	void Update() {
		if(_controls.GetButtonDown("flashlight")) {
			_bulb.enabled = !_bulb.enabled;
		}
	}
}
