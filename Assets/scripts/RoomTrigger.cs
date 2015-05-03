using UnityEngine;
using System.Collections;

public class RoomTrigger : MonoBehaviour {

//	public Room containingRoom;

	public string roomName;

	void OnTriggerEnter(Collider tgt) {
		Debug.Log("RoomTrigger/OnTriggerEnter, roomName = " + this.roomName);
		EventCenter.Instance.enterRoom(this.roomName);
//		containingRoom.roomTriggered(this.roomName);
	}

}
