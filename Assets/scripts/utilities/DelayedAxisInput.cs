using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

public class DelayedAxisInput {
	
	private float _previousTime; 
	
	public static bool Check(string axis = "both", float horizontal = 0, float vertical = 0) {
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

	private bool _checkPreviousTime() {
		var changed = false;
		float now = Time.realtimeSinceStartup;
		if(-(_previousTime - now) > INPUT_DELAY) {
			changed = true;
		}
		_previousTime = now;
		return changed;
	}

}