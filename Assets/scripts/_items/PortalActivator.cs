using UnityEngine;
using System.Collections;
using Polyworks;

public class PortalActivator : CollectableItem {

	public float secondsToCharge = 5.0f; 

	private float _isUsableCounter; 

	public override void Use () {
		if (data.isUsable) {
			data.isUsable = false;
			_isUsableCounter = secondsToCharge;
		}
	}

	private void Awake() {
		data.isUsable = false;
		data.isDroppable = false;
		_isUsableCounter = secondsToCharge;
	}

	private void FixedUpdate() {
//		Debug.Log ("PortalActivator: " + Time.deltaTime + ", _isUsableCounter = " + _isUsableCounter);
		if (!data.isUsable) {
			_isUsableCounter -= Time.deltaTime;
			if (_isUsableCounter <= 0) {
				Debug.Log (" now usable");
				data.isUsable = true;
				_isUsableCounter = 0;
			}
		}
	}

	private void OnDestroy() {

	}
}
