using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {

	public bool isStartingRoom = false;
	public string roomName;

	// Use this for initialization
	void Start () {
		if(isStartingRoom) {
			EventCenter.Instance.enterRoom(this.roomName);
		}
	}
	
	public void roomTriggered(string name) {
		var ec = EventCenter.Instance;

		if(name == this.roomName) {
			ec.exitRoom(this.roomName);
		} else {
			ec.enterRoom(this.roomName);
		}
	}
}
