using UnityEngine;

public class Room : MonoBehaviour {

	public bool _isStartingRoom;
	public string _roomName;

	// Use this for Initialization
	void Start() {
		if(_isStartingRoom) {
			EventCenter.Instance.EnterRoom(_roomName);
		}
	}
	
	public void RoomTriggered(string name) {
		var ec = EventCenter.Instance;

		if(name == _roomName) {
			ec.ExitRoom(_roomName);
		} else {
			ec.EnterRoom(_roomName);
		}
	}
}
