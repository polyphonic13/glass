using UnityEngine;

public class Room : MonoBehaviour {

	public Vector3 startingPosition;
	public Quaternion startingRotation; 

	public string _roomName;

	public void RoomTriggered(string name) {
		var ec = EventCenter.Instance;

		if(name == _roomName) {
			ec.ExitRoom(_roomName);
		} else {
			ec.EnterRoom(_roomName);
		}
	}
}
