using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {

	public bool isStartingRoom = false;
	public string roomName;

	// Use this for initialization
	void Start () {
		if(isStartingRoom) {
			EventCenter.Instance.EnterRoom(roomName);
		}
	}
	
	public void roomTriggered(string name) {
		var ec = EventCenter.Instance;

		if(name == roomName) {
			ec.ExitRoom(roomName);
		} else {
			ec.EnterRoom(roomName);
		}
	}
}
