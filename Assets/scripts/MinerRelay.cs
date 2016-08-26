using UnityEngine;
using System.Collections;
using Polyworks;

public class MinerRelay : Item {

	private AnimationSwitch _animationSwitch;
	private StringEventSwitch _eventSwitch;

	private bool isClosed = false;

	public override void Actuate(Inventory inventory = null) {
		if (isEnabled) {
			isClosed = !isClosed;
			_actuate ();
		}
	}

	private void Awake() {
		_animationSwitch = GetComponent<AnimationSwitch> ();
		_eventSwitch = GetComponent<StringEventSwitch> ();
	}

	private void _actuate() {
		if (_animationSwitch != null) {
			_animationSwitch.Actuate ();
		}
		if(_eventSwitch != null && isClosed) {
			_eventSwitch.Actuate ();
		}
	}
}

