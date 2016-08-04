using UnityEngine;
using System.Collections;
using Polyworks;

public class RoomItem : MonoBehaviour {

	public string containingRoom; 

	public bool IsRoomActive { get; set; } 

	private Item _item; 

	public virtual void Init() {
		_item = GetComponent<Item> () as Item;

		if(transform.tag == "persistentItem" || containingRoom == "") {
//			IsRoomActive = true;
			_item.isEnabled = true;
		} else {
//			IsRoomActive = false;
			_item.isEnabled = false;
			var eventCenter = EventCenter.Instance;
//			eventCenter.OnRoomEntered += OnRoomEntered;
		}
	}

	public virtual void OnRoomEntered(string room) {
		if (room == containingRoom) {
//			IsRoomActive = true;
			_item.isEnabled = true;
		} else {
			_item.isEnabled = false;
//			IsRoomActive = false;
		}
	}

	private void Awake () {
		Init ();
	}
}
