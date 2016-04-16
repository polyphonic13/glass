using UnityEngine;

public class RoomTrigger : MonoBehaviour {

	public string _roomName;

	void OnTriggerEnter(Collider tgt) {
//		Debug.Log("RoomTrigger/OnTriggerEnter, _roomName = " + _roomName + " tgt tag = " + tgt.gameObject.tag);
		EventCenter.Instance.EnterRoom(_roomName);
	}

}
