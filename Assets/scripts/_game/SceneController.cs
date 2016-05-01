using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour {

	public Room[] rooms;

	private int _startingRoom;

	void Awake() {
		/*
		int targetRoom = GameControl.Instance.targetRoom;
//		Debug.Log ("scene manager awake, targetRoom = " + targetRoom);
		if(targetRoom > -1 && targetRoom < rooms.Length) {
			Room room = rooms[targetRoom];
//			Debug.Log("entering room: " + room.name);
			EventCenter.Instance.EnterRoom(room.name);

			PositionAtStart(room);
		}
		*/
	}

	public void PositionAtStart(Room room) {
		Transform player = GameObject.Find ("player").transform;
//		Debug.Log("starting position = " + room.startingPosition + ", starting rotation = " + room.startingRotation);
		player.position = room.startingPosition;
		player.rotation = room.startingRotation;
	}
}
