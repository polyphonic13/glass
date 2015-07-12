using UnityEngine;
using System.Collections;

public class RoomElement : MonoBehaviour {

	public string containingRoom;

	public bool isRoomActive { get; set; }

	// Use this for initialization
	void Awake () {
		init ();
	}

	public void Init() {
		var ec = EventCenter.Instance;

		ec.OnRoomEntered += OnRoomEntered;
		ec.OnRoomExited	 += OnRoomExited;
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void OnRoomEntered(string room) {
//		Debug.Log("RoomElement/OnRoomEntered, room = " + room + ", containingRoom = " + containingRoom);
		if(room == containingRoom) {
			Debug.Log (name + "ACTIVATED");
			isRoomActive = true;
		} else {
			isRoomActive = false;
		}
	}

	public void OnRoomExited(string room) {
		if(room == containingRoom) {
			isRoomActive = false;
		}
	}
}
