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
		}
	}

	public virtual void OnRoomEntered(string room) {
		if (room == _containingRoom) {
			IsRoomActive = true;
		} else {
			IsRoomActive = false;
		}
	}

	private void Awake () {
		Init ();
	}
}
