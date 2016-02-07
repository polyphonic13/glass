using UnityEngine;

public class Flashlight : MonoBehaviour {

	private Light _bulb;
	void Awake() {
		_bulb = gameObject.GetComponent<Light>();
		_bulb.enabled = false;
	}
	
	public void Actuate() {
		_bulb.enabled = !_bulb.enabled;
	}
}
