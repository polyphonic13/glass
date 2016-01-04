using UnityEngine;
using System.Collections;

public class MoonController : MonoBehaviour {

	public bool isNightAtStart = false;

	private Light _moon;

	// Use this for initialization
	void Awake () {
		_moon = this.gameObject.GetComponent<Light> ();
//		_moon.enabled = isNightAtStart;
		EventCenter.Instance.OnDayNightChange += this.OnDayNightChange;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnDayNightChange(string state) {
		if (state == "night") {
			Debug.Log("enabling moon");
//			_moon.enabled = true;
		} else {
			Debug.Log("disabling moon");
//			_moon.enabled = false;
		}
	}
}
