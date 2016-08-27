using UnityEngine;
using System.Collections;

public class VerticalMovementArea : MonoBehaviour {
	public Transform top;
	public Transform bottom; 
	public Transform left;
	public Transform right;

	private bool _isUpDownEnabled = false;
	private bool _isLeftRightEnabled = false; 

	void Start () {
		if (top != null && bottom != null) {
			_isUpDownEnabled = true;
		}
		if (left != null && right != null) {
			_isLeftRightEnabled = true;
		}

	}

	void OnDrawGizmos() {
		Gizmos.color = Color.grey;
		if (top != null) {
			Gizmos.DrawWireCube (top.position, top.localScale);
		}
		if (bottom != null) {
			Gizmos.DrawWireCube (bottom.position, bottom.localScale);
		}
		if (left != null) {
			Gizmos.DrawWireCube (left.position, left.localScale);
		}
		if (right != null) {
			Gizmos.DrawWireCube (right.position, right.localScale);
		}
		// Gizmos.color = Color.blue;
		// Gizmos.DrawWireCube (this.transform.position, this.transform.localScale);
	}

	public bool GetIsUpDownEnabled() {
		return _isUpDownEnabled;
	}

	public bool GetIsLeftRightEnabled() {
		return _isLeftRightEnabled;
	}
}
