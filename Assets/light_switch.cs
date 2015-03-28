using UnityEngine;
using System.Collections;

public class light_switch : MonoBehaviour {

	private Light _bulb;

	// Use this for initialization
	void Awake() {
		_bulb = GetComponent<Light>();
		_bulb.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.L)) {
			_bulb.enabled = !_bulb.enabled;
		}
	}
}
