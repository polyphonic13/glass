using UnityEngine;

public class RoomTrigger : MonoBehaviour {

//	public Room containingRoom;

	public string _roomName;

	void OnTriggerEnter(Collider tgt) {
		Debug.Log("RoomTrigger/OnTriggerEnter, _roomName = " + _roomName);
		EventCenter.Instance.EnterRoom(_roomName);
//		containingRoom.roomTriggered(_roomName);
	}

}
