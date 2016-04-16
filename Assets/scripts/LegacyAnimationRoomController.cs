using UnityEngine;
using System.Collections;

public class LegacyAnimationRoomController : RoomItem {

	private LegacyAnimationController _animationController; 

	public override void OnRoomEntered (string room)
	{
		base.OnRoomEntered (room);
//		Debug.Log ("LegacyAnimationRoomController/OnRoomEntered, room = " + room + ", IsRoomActive = " + IsRoomActive);
		if (IsRoomActive) {
			_roomEntered ();
		} else {
			_roomExited ();
		}
	}

	void Awake() {
		base.Init ();
		_animationController = GetComponent<LegacyAnimationController> ();
	}

	private void _roomEntered() {
		if (_animationController.GetIsActive()) {
			_animationController.Resume();
		}
	}

	private void _roomExited() {
		if(_animationController.GetIsActive()) {
			_animationController.Pause();
		}
	}


}
