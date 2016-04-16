using UnityEngine;
using System.Collections;

public class RoomItem : MonoBehaviour {

	[SerializeField] private string _containingRoom; 

	public bool IsRoomActive { get; set; } 

	public virtual void Init() {
		
		if(transform.tag == "persistentItem" || _containingRoom == "") {
			IsRoomActive = true;
		} else {
			IsRoomActive = false;

			var eventCenter = EventCenter.Instance;
			eventCenter.OnRoomEntered += OnRoomEntered;
//			eventCenter.OnRoomExited += OnRoomExited;
		}
	}

	public virtual void OnRoomEntered(string room) {
		Debug.Log ("RoomItem[" + this.name + "]/OnRoomEntered");
		if (room == _containingRoom) {
//			Debug.Log("activating: " + room);
			IsRoomActive = true;
		} else {
			IsRoomActive = false;
		}
	}

//	public virtual void OnRoomExited(string room) {
//		if(room == _containingRoom) {
//			Debug.Log("deactivating: " + room);
//			IsRoomActive = false;
//		}
//	}

	private void Awake () {
		Init ();
	}
}
