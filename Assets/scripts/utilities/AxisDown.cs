using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

public class AxisDown : MonoBehaviour {

	private int _horizontalAxisInput;
	private int _verticalAxisInput;

	private bool _isHorizontalDown; 
	private bool _isVerticalDown; 

	void Update () {
		float horizontal = CrossPlatformInputManager.GetAxisRaw("Horizontal");
		float vertical = CrossPlatformInputManager.GetAxisRaw("Vertical");

		_isAxisInUse(horizontal, _horizontalAxisInput, _isHorizontalDown);
		_isAxisInUse(vertical, _verticalAxisInput, _isVerticalDown);
	}

	private void _isAxisInUse(float val, int isInUse, bool isDown) {
		if(val != 0) {
			if(isDown == false) {
				if(val < 0) {
					isInUse = -1;
				} else {
					isInUse = 1;
				}
				Debug.Log("DO SOMETHING NOW");
				isDown = true;
			}
		} else if(val == 0) {
			isInUse = 0;	
			isDown = false;
		}
	}

	public int GetAxisInput(string axis) {
		int input;
		if(axis == "vertical") {
			input = _verticalAxisInput;
		} else {
			input = _horizontalAxisInput;
		}
		return input;
	}

	public bool GetAxisDown(string axis) {
		if(axis == "vertical") {
			return _isHorizontalDown;
		} else {
			return _isVerticalDown;
		}
	}

}
