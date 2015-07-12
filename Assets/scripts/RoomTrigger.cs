using UnityEngine;
using System.Collections;

public class RoomTrigger : MonoBehaviour {

//	public Room containingRoom;

	public string roomName;

	void OnTriggerEnter(Collider tgt) {
		Debug.Log("RoomTrigger/OnTriggerEnter, roomName = " + roomName);
		EventCenter.Instance.EnterRoom(roomName);
//		containingRoom.roomTriggered(roomName);
	}

}
