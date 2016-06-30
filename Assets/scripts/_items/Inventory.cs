﻿using UnityEngine;
using System.Collections;
using Polyworks;

public class Inventory : MonoBehaviour {

	[SerializeField] private int _maxItems = 15;

	private Transform _backpack;
	private Transform _leftHand;
	private Transform _rightHand;

	private Hashtable _items;
	private Hashtable _itemList;

	private static Inventory _instance;
	private Inventory() {}

	public void Init() {
//		if (Game.Instance.gameData.items != null) {
//			Debug.Log ("Inventory/Init, getting items from GameControl");
//			_items = Game.Instance.gameData.items as Hashtable;
//			Debug.Log("_items.Count = " + _items.Count);
//			if (_items.Count > 0) {
//				foreach(CollectableItem item in _items.Values) {
//					EventCenter.Instance.InventoryAdded(item.name);
//				}
//			}
//		} else {
//			_items = new Hashtable ();
//			_itemList = new Hashtable ();
//		}
	}

	public void InitPlayer() {
		Transform player = GameObject.Find ("player").transform;
		_backpack = player.Find ("backpack");
		_leftHand = player.Find ("player_head_camera/left_hand");
		_rightHand = player.Find ("player_head_camera/right_hand");
		EventCenter.Instance.OnInspectItem += OnInspectItem;
	}

	public static Inventory Instance {
		get {
			if(_instance == null) {
				_instance = GameObject.FindObjectOfType(typeof(Inventory)) as Inventory;      
			}
            return _instance;
        }
    }

	public bool HasItem(string key) {
		return(_items.Contains(key)) ? true : false;
	}

	public bool AddItem(CollectableItem item) {
		var isAdded = true;
		 Debug.Log("Inventory/AddItem, item = " + item.name);
		if(HasItem(item.name)) {
			// increment count of item type
		} else if(_items.Count < _maxItems) {
			string message;

			if (item.name != "flashlight") {
				_items.Add (item.name, item);
				message = item.data.itemName + " Added to inventory";
//				Game.Instance.hasFlashlight = true;
			} else {
				message = item.data.itemName + " Collected";
			}
			item.Collect();
			
			EventCenter.Instance.AddNote(message);
			EventCenter.Instance.InventoryAdded(item.name, 1);
		} else {
			isAdded = false;
			EventCenter.Instance.AddNote("No more room for: " + item.data.itemName);
		}
		return isAdded;
	}

	public void OnInspectItem(bool isInspecting, string key) {
		if (HasItem (key)) {
			var item = _items[key] as CollectableItem;
			if (isInspecting) {
				ItemInspector.Instance.AddTarget (item.transform, item.data.itemName, item.description);
			} else {
				ItemInspector.Instance.RemoveTarget ();
			}
		}
	}

	public void UseItem(string key) {
		if (HasItem (key)) {
			var item = _items[key] as CollectableItem;
			if(item.data.isUsable) {
				item.Use();
			} else {
				EventCenter.Instance.CloseInventoryUI ();
				EventCenter.Instance.AddNote(item.data.itemName + " can not be used here");
			}
		}
	}

	public void RemoveItem(string key, bool useGravity = true) {
//		Debug.Log("Inventory/RemoveItem, key = " + key);
		if(HasItem(key)) {
			var item = _items[key] as CollectableItem;
//			item.isCollected = false;
			item.Drop(useGravity);
			_items.Remove(key);
			EventCenter.Instance.InventoryRemoved(item.name, 1);
		}
	}

	public CollectableItem GetItem(string key) {
		if(HasItem(key)) {
			return _items[key] as CollectableItem;
		} else {
			return null;
		}
	}

	public Hashtable GetAll() {
		return _items;
	}
}
