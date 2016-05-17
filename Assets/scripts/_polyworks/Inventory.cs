using UnityEngine;
using System.Collections;

namespace Polyworks {

	public class Inventory : MonoBehaviour
	{
		private Hashtable _items;

		private static Inventory _instance;
		private Inventory() {}

		public static Inventory Instance {
			get {
				if(_instance == null) {
					_instance = GameObject.FindObjectOfType(typeof(Inventory)) as Inventory;      
				}
				return _instance;
			}
		}

		public void Init(Hashtable items = null) {
			Debug.Log ("Inventory/Init, _items = " + _items);
			if (items != null) {
				_items = items as Hashtable;
				if (_items.Count > 0) {
					foreach(ItemData item in _items.Values) {
						InsertItem(item);
					}
				}
			} else {
				Debug.Log ("creating new items Hashtable");
				_items = new Hashtable ();
			}
		}

		public virtual void Add(ItemData item) {
			InsertItem (item);
			EventCenter.Instance.AddNote(item.itemName + " Collected");
		}

		public virtual void InsertItem(ItemData item) {
			item.isCollected = true;
			_items.Add (item.itemName, item);
			EventCenter.Instance.AddInventory(item.itemName);
		}

		public virtual void Remove(string name) {
			if (Contains (name)) {
				var data = _items [name] as ItemData;
				_items.Remove (name);

				if (data.isDroppable && data.itemObject != null) {
					GameObject itemObject = (GameObject) Instantiate (data.itemObject, transform.position, transform.rotation);
				}
			}
		}

		public bool Contains(string key) {
			return(_items.Contains(key)) ? true : false;
		}

		public Hashtable GetAll() {
			return _items;
		}
	}
}