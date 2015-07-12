using UnityEngine;

public class Flashlight : MonoBehaviour {

	private Light _bulb;

	// Use this for initialization
	void Start () {
		_bulb = gameObject.GetComponent<Light>();
		_bulb.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.F)) {
			_bulb.enabled = !_bulb.enabled;
		}
	}
}
