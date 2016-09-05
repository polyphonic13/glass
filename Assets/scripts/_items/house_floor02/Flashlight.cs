using UnityEngine;
using Polyworks;

public class Flashlight : CollectableItem {
	public string parentName;
	public GameObject model;
	private Light _bulb;

	public void OnActuateFlashlight() {
		Debug.Log ("Flashlight/OnActuateFlashlight");
		_bulb.enabled = !_bulb.enabled;
	}

	public override void Actuate() {
		Debug.Log ("Flashlight/Actuate");
		model.SetActive (false);

		GameObject playerHead = GameObject.Find (parentName);
		this.transform.parent = playerHead.transform;
	}

	private void Awake() {
		_bulb = gameObject.GetComponent<Light>();
		_bulb.enabled = false;

		EventCenter.Instance.OnActuateFlashlight += OnActuateFlashlight;
	}

	private void OnDestroy() {
		EventCenter ec = EventCenter.Instance;
		if (ec != null) {
			ec.OnActuateFlashlight -= OnActuateFlashlight;
		}
	}
}
