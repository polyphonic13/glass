﻿using UnityEngine;
using System.Collections;

namespace Polyworks {

	public class Inventory : MonoBehaviour
	{
		private Hashtable _items;
		private EventCenter _eventCenter; 

		private bool _isPlayerInventory; 

		public void Init(Hashtable items = null, bool isPlayerInventory = false) {
			Debug.Log ("Inventory/Init, items.Count = " + items.Count);
			_isPlayerInventory = isPlayerInventory;

			if (items != null && items.Count > 0) {
				_items = new Hashtable();
				foreach (ItemData itemData in items.Values) {
					Add (itemData, false);
				}
			} else if (_items == null) {
				_items = new Hashtable ();
			}
		}

		public virtual void Add(ItemData item, bool increment = true) {
			if (!Contains (item.itemName)) {
				_items.Add (item.itemName, item);
			}
			ItemData itemData = Get (item.itemName) as ItemData;
			if (increment) {
				itemData.count++;
			}
			if (_isPlayerInventory) {
				_eventCenter.InventoryAdded (itemData.itemName, itemData.count, _isPlayerInventory);
				_eventCenter.AddNote (itemData.itemName + " Added");
			}
		}

		public virtual ItemData Remove(string name) {
			if (!Contains (name)) {
				return null;
			} else {
				var data = _items [name] as ItemData;

				if (data != null && data.count > 0) {
					data.count--;

					if (data.count == 0) {
						_items.Remove (name);
					}
					_eventCenter.InventoryRemoved (data.itemName, data.count);
					return data;

				} else {
					return null;
				}
			} 
		}

		public virtual void Use(string name) {
			_eventCenter.CloseInventoryUI ();

			ItemData data = Remove (name);
			if (data != null) {
				// Debug.Log ("Inventory/Use, name = " + name + ", prefab = " + data.prefabName + ", isDestroyedOnUse = " + data.isDestroyedOnUse);
				GameObject itemObj = (GameObject) Instantiate (Resources.Load (data.prefabName, typeof(GameObject)), transform.position, transform.rotation);
				Item item = itemObj.GetComponent<Item> ();
				item.data = data;
				item.data.isCollected = false;
				item.Use ();

				if (data.isDestroyedOnUse) {
					// Debug.Log (" kill " + itemObj);
					Destroy (itemObj);
				}
			}
		}

		public bool Contains(string key) {
			return(_items.Contains(key)) ? true : false;
		}

		public virtual ItemData Get(string name) {
			if (Contains (name)) {
				return _items [name] as ItemData;
			} 
			return null;
		}

		public Hashtable GetAll() {
			return _items;
		}

		private void Awake() {
			Debug.Log ("Inventory/Awake");
			_eventCenter = EventCenter.Instance;
		}

		private void OnDestroy() {
			// Debug.Log ("Inventory/OnDestroy");
		}

	}
}