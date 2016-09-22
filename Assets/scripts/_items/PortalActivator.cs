using UnityEngine;
using System.Collections;
using Polyworks;

public class PortalActivator : CollectableItem {

	public float secondsToCharge = 5.0f; 
	public string[] usableScenes; 

	private bool _isInUsableScene = false; 

	private float _isUsableCounter;

	public void OnSceneInitialized(string scene) {
		for (var i = 0; i < usableScenes.Length; i++) {
			if (usableScenes [i] == scene) {
				_isInUsableScene = true;
				break;
			}
		}
	}

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

		EventCenter.Instance.OnSceneInitialized += OnSceneInitialized;
	}

	private void FixedUpdate() {
//		Debug.Log ("PortalActivator: " + Time.deltaTime + ", _isUsableCounter = " + _isUsableCounter);
		if (_isInUsableScene && !data.isUsable) {
			_isUsableCounter -= Time.deltaTime;
			if (_isUsableCounter <= 0) {
				Debug.Log ("PortalActivator now usable");
				data.isUsable = true;
				_isUsableCounter = 0;
			}
		}
	}

	private void OnDestroy() {
		EventCenter ec = EventCenter.Instance;
		if (ec != null) {
			ec.OnSceneInitialized -= OnSceneInitialized;
		}
	}
}
