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

		ec.OnRoomEntered += this.OnRoomEntered;
		ec.OnRoomExited	 += this.OnRoomExited;
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void OnRoomEntered(string room) {
//		Debug.Log("RoomElement/OnRoomEntered, room = " + room + ", this.containingRoom = " + this.containingRoom);
		if(room == this.containingRoom) {
			Debug.Log (this.name + "ACTIVATED");
			this.isRoomActive = true;
		} else {
			this.isRoomActive = false;
		}
	}

	public void OnRoomExited(string room) {
		if(room == this.containingRoom) {
			this.isRoomActive = false;
		}
	}
}
