using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

public class DelayedAxisInput {
	
	private static float _previousTime; 
	private const float INPUT_DELAY = .03f;

	public static bool Check(string axis, float horizontal, float vertical) {
		bool changed = false;
		horizontal = CrossPlatformInputManager.GetAxisRaw("Horizontal");
		vertical = CrossPlatformInputManager.GetAxisRaw("Vertical");

		if(axis == "both" && (horizontal != 0 || vertical != 0)) {
			changed = _checkPreviousTime();
		} else if(axis == "horizontal" && horizontal != 0) {
			changed = _checkPreviousTime();
		} else if(axis == "vertical" && vertical != 0) {
			changed = _checkPreviousTime();
		}

		return changed;
	}

	private static bool _checkPreviousTime() {
		var changed = false;
		float now = Time.realtimeSinceStartup;
		if(-(_previousTime - now) > INPUT_DELAY) {
			changed = true;
		}
		_previousTime = now;
		return changed;
	}

}