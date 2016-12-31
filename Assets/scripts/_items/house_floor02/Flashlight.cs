using UnityEngine;
using Polyworks;

public class Flashlight : CollectableItem {
	public const string COLLECTED = "isFlashlightCollected";
	public string parentName;
	public GameObject model;
	private Light _bulb;

	public void OnCollectFlashlight() {
		Debug.Log ("Flashlight[" + this.name + "]/OnCollectFlashlight");
		this.data.isCollected = true;
	}

	public void OnEnableFlashlight() {
		Debug.Log ("Flashlight["+this.name+"]/OnEnableFlashlight, isCollected = " + this.data.isCollected);
		if (this.data.isCollected) {
			_bulb.enabled = !_bulb.enabled;
		}
	}

	public override void Actuate() {
//		model.SetActive (false);
//		Quaternion rotation = new Quaternion(0, 0, 0, 0);
//		GameObject playerHead = GameObject.Find (parentName);
//		this.transform.rotation = rotation;
//		this.transform.parent = playerHead.transform;
//		this.data.isCollected = true;
		Game.Instance.SetFlag(COLLECTED, true);
		EventCenter ec = EventCenter.Instance;
		ec.CollectFlashight();
		ec.ChangeItemProximity(this, false);
		ec.AddNote (this.displayName + " added");

		_removeListeners ();
		Destroy (this.gameObject);
	}

	private void Awake() {
		_bulb = gameObject.GetComponent<Light>();
		_bulb.enabled = false;

		EventCenter ec = EventCenter.Instance;
		ec.OnCollectFlashlight += OnCollectFlashlight;
		ec.OnEnableFlashlight += OnEnableFlashlight;
	}

	private void OnDestroy() {
		_removeListeners ();
	}

	private void _removeListeners() {
		EventCenter ec = EventCenter.Instance;
		if (ec != null) {
			ec.OnCollectFlashlight -= OnCollectFlashlight;
			ec.OnEnableFlashlight -= OnEnableFlashlight;
		}
	}
}
