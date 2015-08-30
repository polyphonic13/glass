using UnityEngine;

public class Flashlight : MonoBehaviour {

	private Light _bulb;

	void Start() {
		_bulb = gameObject.GetComponent<Light>();
		_bulb.enabled = false;
	}
	
	void Update() {
		if(Input.GetKeyDown(KeyCode.F)) {
			_bulb.enabled = !_bulb.enabled;
		}
	}
}
