using UnityEngine;
using System.Collections;
using Polyworks; 

public class ToyChest : Item {
	public static string RABBIT_HUNT_ADD_EVENT = "rabbitHuntToyAdded";
	public static string RABBIT_HUNT_COMPLETE_EVENT = "rabbitHuntCompleted"; 

	public GameObject[] collectedToys; 

	private int _collected = 0;
	private int _expected = 2;

	private bool _isLidOpen = false; 

	#region handlers
	public void OnStringEvent(string type, string value) {
		Debug.Log ("ToyChest/OnStringEvent, type = " + type + ", value = " + value + ", _isLidOpen = " + _isLidOpen);
		if (type == RABBIT_HUNT_ADD_EVENT) {
			if (!_isLidOpen) {
				// dispatch an event to open the lid first
				EventCenter.Instance.InvokeStringEvent("open_toychest_lid", "");
				// toggle once before event received
				_isLidOpen = !_isLidOpen;
			}
			for (int i = 0; i < collectedToys.Length; i++) {
				Debug.Log (" collectedToys[" + i + "].name = " + collectedToys [i].name);
				if (collectedToys [i].name == value) {
					collectedToys [i].SetActive (true);
					_collected++;
					break;
				}
			}
			Debug.Log (" _collected = " + _collected + ", _expected = " + _expected);
			if (_collected == _expected) {
				EventCenter.Instance.InvokeStringEvent (RABBIT_HUNT_COMPLETE_EVENT, "");
			}
		} else if (type == "toychest_lid_collider") {
			_isLidOpen = !_isLidOpen;
			Debug.Log (" is lid open now: " + _isLidOpen);
		}
	}
	#endregion

	#region public methods
	public override void Enable () {
		base.Enable ();
		EventCenter.Instance.OnStringEvent += OnStringEvent;
	}

	public override void Disable () {
		base.Disable ();
		EventCenter.Instance.OnStringEvent -= OnStringEvent;
	}
	#endregion

	#region private methods
	private void Awake() {
		for (int i = 0; i < collectedToys.Length; i++) {
			collectedToys [i].SetActive (false);
		}
	}

	private void OnDestroy() {
		EventCenter ec = EventCenter.Instance;
		if (ec != null) {
			EventCenter.Instance.OnStringEvent -= OnStringEvent;
		}
	}
	#endregion
}
