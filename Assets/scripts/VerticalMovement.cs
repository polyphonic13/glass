using UnityEngine;
using System.Collections;
using UnitySampleAssets.Characters.FirstPerson; 

public class VerticalMovement : MonoBehaviour {
	public float climbDownThreshold = -0.4f;

	private float _verticalMovement = 0;
	private float _horizontalMovement = 0;

	private VerticalMovementArea _currentVerticalMovementArea;

	private Vector3 _climbDirection = Vector3.zero;

	private bool _isAttachedToVerticalMovementArea;
	private bool _isInClimbTrigger;

	private bool _isUpDownEnabled = false;
	private bool _isLeftRightEnabled = false; 

	private Player _player; 
	private Camera _mainCamera;

	private Vector3 _movement;

	public Vector3 GetMovement(float horizontal, float vertical, bool isJumpPressed, bool isClimbPressed) {
//		Debug.Log("VerticalMovement/Move, horizontal = " + horizontal + ", vertical = " + vertical + ", isJumpPressed = " + isJumpPressed + ", isClimbPressed = " + isClimbPressed);
		// stop vertical movement is jump was pressed
		_movement = Vector3.zero;

		if (isJumpPressed) {
			if (_isAttachedToVerticalMovementArea) {
				_detachFromVerticalMovementArea ();
			}
		} else {
			// 
			if (_isInClimbTrigger) {
				if (isClimbPressed) {
					// move vertically
//					Debug.Log("in climb trigger, move vertically");
					_moveVertically (horizontal, vertical);
				} else {
					// move normally
//					Debug.Log("in climb trigger, move normally");
					_movement.y = 0;
					_movement.x = horizontal;
					_movement.z = vertical;
				}
			} else {
				// move vertically
//				Debug.Log("move vertically");
				if (isClimbPressed) {
					_moveVertically (horizontal, vertical);
//				} else {
//					if (_isAttachedToVerticalMovementArea) {
//						_detachFromVerticalMovementArea ();
//					}
				}
			}
		}

		return _movement;
	}

	public bool GetIsAttachedToVerticalArea() {
		return _isAttachedToVerticalMovementArea;
	}

	public void Detach() {
		Debug.Log ("VerticalMovement/Detach");
		if (_currentVerticalMovementArea != null) {
			_detachFromVerticalMovementArea ();
		}
	}

	private void Awake() {
		_player = GetComponent<Player> ();
		_mainCamera = Camera.main;
	}

	#region trigger handlers
	private void OnTriggerEnter(Collider tgt) {
		if(tgt.gameObject.tag == "verticalMovementTrigger") {
			Debug.Log ("triggered vertical movement trigger");
			_isInClimbTrigger = true;
		}

		if (tgt.gameObject.tag == "verticalMovementArea") {
//			Debug.Log ("triggered vertical movement area");
			_attachToVerticalMovementArea (tgt.gameObject);
		}
	}

	private void OnTriggerExit(Collider tgt) {
		if(tgt.gameObject.tag == "verticalMovementTrigger") {
			Debug.Log ("exited vertical movement trigger");
			_isInClimbTrigger = false;
		}

		if (tgt.gameObject.tag == "verticalMovementArea") {
//			Debug.Log ("exited vertical movement area");
			_detachFromVerticalMovementArea ();
		}
	}

	private void _attachToVerticalMovementArea(GameObject tgt) {
		Debug.Log ("Attaching player");
		_currentVerticalMovementArea = tgt.GetComponent<VerticalMovementArea> ();
		_isUpDownEnabled = _currentVerticalMovementArea.GetIsUpDownEnabled ();
		_isLeftRightEnabled = _currentVerticalMovementArea.GetIsLeftRightEnabled ();
		_isAttachedToVerticalMovementArea = true;
		_player.SetClimbing (true);
	}

	private void _detachFromVerticalMovementArea() {
		_currentVerticalMovementArea = null;
		_isUpDownEnabled = false;
		_isUpDownEnabled = false;
		_isAttachedToVerticalMovementArea = false;
		_player.SetClimbing (false);
	}
	#endregion

	private void _moveVertically(float horizontal, float vertical) {
		_movement.z = 0;
		if (_isUpDownEnabled) {
			_movement.y = vertical;
			_movement.y *= (_mainCamera.transform.forward.y > climbDownThreshold) ? 1 : -1;

		} else {
			_movement.y = 0;
		}
		if (_isLeftRightEnabled) {
			_movement.x = horizontal;
		} else {
			_movement.x = 0;
		}
	}

}
