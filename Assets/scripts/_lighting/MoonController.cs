using UnityEngine;
using System.Collections;

public class MoonController : MonoBehaviour {

	public Gradient moonlightColor;
	public float maxMoonlight = 1f;
	public float minMoonlight = 0;
	public float sunOffset = 45f;
		
	private Light _moon;

	// Use this for initialization
	void Awake () {
		_moon = this.gameObject.GetComponent<Light> ();
//		EventCenter.Instance.OnDayNightChange += this.OnDayNightChange;
	}
	
	public void UpdateCycle (Vector3 speed, float skySpeed) {
		this.transform.Rotate (speed * Time.deltaTime * skySpeed);
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
