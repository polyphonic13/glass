using UnityEngine;
using System.Collections;

public class RoomElement : MonoBehaviour {

	public string containingRoom;

	public bool isRoomActive { get; set; }

	// Use this for initialization
	void Awake () {
		init ();
	}

	public void init() {
		var ec = EventCenter.Instance;

		ec.onRoomEntered += this.onRoomEntered;
		ec.onRoomExited	 += this.onRoomExited;
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void onRoomEntered(string room) {
		Debug.Log("RoomElement/onRoomEntered, room = " + room + ", this.containingRoom = " + this.containingRoom);
		if(room == this.containingRoom) {
			Debug.Log ("ACTIVATED");
			this.isRoomActive = true;
		}
	}

	public void onRoomExited(string room) {
		if(room == this.containingRoom) {
			this.isRoomActive = false;
		}
	}
}
