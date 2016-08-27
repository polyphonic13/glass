using UnityEngine;
using System.Collections;
using Polyworks;

public class CrystalHammerControls : Item {

	private AnimationSwitch _animationSwitch;

	private bool isClosed = false;

	public override void Actuate() {
		if (isEnabled) {
			_actuate ();
		}
	}

	private void Awake() {
		_animationSwitch = GetComponent<AnimationSwitch> ();
	}

	private void _actuate() {
		if (_animationSwitch != null) {
			_animationSwitch.Actuate ();
		}
	}
}


