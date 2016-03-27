using UnityEngine;
using System.Collections;

public class VerticalMovementArea : MonoBehaviour {
	public Transform top;
	public Transform bottom; 
	public Transform left;
	public Transform right;

	private bool _isUpDownEnabled = false;
	private bool _isLeftRightEnabled = false; 

//	private Vector3 _climbDirection; 

	void Start () {
		if (top != null && bottom != null) {
			_isUpDownEnabled = true;
//			_climbDirection = top.transform.position - bottom.transform.position;
		}
		if (left != null && right != null) {
			_isLeftRightEnabled = true;
		}

	}

	public bool GetIsUpDownEnabled() {
		return _isUpDownEnabled;
	}

	public bool GetIsLeftRightEnabled() {
		return _isLeftRightEnabled;
	}
}
