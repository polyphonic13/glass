using UnityEngine;

public class RoomElement : MonoBehaviour {

	public string _containingRoom;

	public bool IsRoomActive { get; set; }

	// Use this for Initialization
	void Awake() {
		Init();
	}

	public void Init() {
		var ec = EventCenter.Instance;

		ec.OnRoomEntered += OnRoomEntered;
		ec.OnRoomExited	 += OnRoomExited;
	}

	// Update is called once per frame
	void Update() {
	
	}

	public void OnRoomEntered(string room) {
//		Debug.Log("RoomElement/OnRoomEntered, room = " + room + ", _containingRoom = " + _containingRoom);
		IsRoomActive = (room == _containingRoom);
		// if(room == _containingRoom) {
		// 	IsRoomActive = true;
		// } else {
		// 	IsRoomActive = false;
		// }
	}

	public void OnRoomExited(string room) {
		if(room == _containingRoom) {
			IsRoomActive = false;
		}
	}
}
