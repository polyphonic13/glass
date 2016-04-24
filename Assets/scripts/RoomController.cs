using UnityEngine;
using System.Collections;

public class RoomController : RoomItem {

	public TargetController[] targetControllers; 

	public override void OnRoomEntered (string room)
	{
		base.OnRoomEntered (room);
//		Debug.Log ("RoomController/OnRoomEntered, room = " + room + ", IsRoomActive = " + IsRoomActive);
		if (IsRoomActive) {
			Debug.Log ("RoomController[" + this.name + "] _resumeTargets");
			_resumeTargets ();
		} else {
			Debug.Log ("RoomController[" + this.name + "] _resumeTargets");
			_pauseTargets ();
		}
	}

	private void Awake() {
		base.Init ();
	}

	private void _pauseTargets() {
		for (int i = 0; i < targetControllers.Length; i++) {
			if (targetControllers [i].GetIsActive ()) {
				targetControllers [i].Pause ();
			}
		}
	}

	private void _resumeTargets() {
		Debug.Log ("RoomController[" + this.name + "]/_resumeTargets");
		for (int i = 0; i < targetControllers.Length; i++) {
			Debug.Log ("\ttargetControllers[" + i + "].GetIsActive = " + targetControllers [i].GetIsActive ());
			if (targetControllers [i].GetIsActive ()) {
				targetControllers [i].Resume ();
			}
		}
	}
}
