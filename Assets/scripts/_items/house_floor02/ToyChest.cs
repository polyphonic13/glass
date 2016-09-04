using UnityEngine;
using System.Collections;
using Polyworks; 

public class ToyChest : Item {
	public static string RABBIT_HUNT_ADD_EVENT = "rabbitHuntToyAdded";
	public static string RABBIT_HUNT_COMPLETE_EVENT = "rabbitHuntCompleted"; 

	public GameObject[] collectedToys; 

	private int _collected = 0;
	private int _expected = 2;

	#region handlers
	public void OnStringEvent(string type, string value) {
		if (type == RABBIT_HUNT_ADD_EVENT) {
			for (int i = 0; i < collectedToys.Length; i++) {
				if (collectedToys [i].name == value) {
					collectedToys [i].SetActive (true);
					_collected++;
					break;
				}
			}
			if (_collected == _expected) {
				EventCenter.Instance.InvokeStringEvent (RABBIT_HUNT_COMPLETE_EVENT, "");
			}
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
