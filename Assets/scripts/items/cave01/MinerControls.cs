using UnityEngine;
using System.Collections;
using Polyworks;

public class MinerControls : Item
{
	public const string RELAY_EVENT = "miner_relay";
	public int expectedEvents = 3; 

	private int _receivedEvents = 0;
	private bool _isCircuitClosed = false; 

	private AnimationSwitch _target;

	public void OnStringEvent(string type, string value) {
		if (type == RELAY_EVENT) {
			_receivedEvents++;

			if (_receivedEvents == expectedEvents) {
				Debug.Log ("MinerControls/OnStringEvent, setting is functional to true");
				// turn on lights / change display
				_isCircuitClosed = true;
			}
		}
	}

	public override void Actuate() {
		if (_isCircuitClosed) {
			_target.Actuate ();
			// turn on lights / change display
		} else {
			EventCenter.Instance.AddNote ("Controls Not Connected");
		}
	}

	private void Awake() {
		_target = GetComponent<AnimationSwitch> ();

		EventCenter ec = EventCenter.Instance;
		ec.OnStringEvent += OnStringEvent;
	}

	private void OnDestroy() {
		EventCenter ec = EventCenter.Instance;
		if (ec != null) {
			ec.OnStringEvent -= OnStringEvent;
		}
	}
}

