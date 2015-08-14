using UnityEngine;

public class RoomElement : MonoBehaviour {

	public string _containingRoom;

	public bool IsRoomActive { get; set; }

	void Awake() {
		Init();
	}

	public void Init() {
		var ec = EventCenter.Instance;

		ec.OnRoomEntered += OnRoomEntered;
		ec.OnRoomExited	 += OnRoomExited;
	}

	void Update() {
	
	}

	public void OnRoomEntered(string room) {
		IsRoomActive =(room == _containingRoom);
	}

	public void OnRoomExited(string room) {
		if(room == _containingRoom) {
			IsRoomActive = false;
		}
	}
}
