using UnityEngine;
using System.Collections;

namespace Polyworks {

	public class Inventory : MonoBehaviour
	{
		private Hashtable _items;

//		private static Inventory _instance;
//		private Inventory() {}
//
//		public static Inventory Instance {
//			get {
//				if(_instance == null) {
//					_instance = GameObject.FindObjectOfType(typeof(Inventory)) as Inventory;      
//				}
//				return _instance;
//			}
//		}

		public void Init(Hashtable items = null) {
//			// Debug.Log ("Inventory/Init");
//			if (_items == null) {
//				// Debug.Log ("making a new hashtable");
//				_items = new Hashtable ();
//			}
//			foreach (ItemData item in items.Values) {
//				if (!Contains (item.itemName)) {
//					InsertItem (item);
//				}
//			}
			// Debug.Log ("Inventory/Init, _items = " + _items + ", items = " + items.Count);
//			if (_items == null) {
//				_items = new Hashtable ();
//			}

			if (items != null && items.Count > 0) {
				_items = items as Hashtable;
//				if (_items.Count > 0) {
//					foreach(ItemData item in _items.Values) {
//						InsertItem(item);
//					}
//				}
			} else if (_items == null) {
				_items = new Hashtable ();
			}
			// Debug.Log ("end of inventory init, _items.Count = " + _items.Count);
		}

		public virtual void Add(ItemData item) {
			InsertItem (item);
			EventCenter.Instance.AddNote(item.itemName + " Collected");
		}

		public virtual void InsertItem(ItemData item) {
			// Debug.Log ("Inventory/InsertItem, item.itemName = " + item.itemName + ", _items = " + _items);
			_items.Add (item.itemName, item);
			EventCenter.Instance.AddInventory (item.itemName);
		}

		public virtual void Remove(string name) {
			if (Contains (name)) {
				var data = _items [name] as ItemData;
				_items.Remove (name);

				if (data.isDroppable && data.prefabName != null) {
					GameObject itemObject = (GameObject) Instantiate (Resources.Load(data.prefabName, typeof(GameObject)), transform.position, transform.rotation);
				}
			}
		}

		public bool Contains(string key) {
			return(_items.Contains(key)) ? true : false;
		}

		public Hashtable GetAll() {
			// Debug.Log ("Inventory/GetAll, _items.Count = " + _items.Count);
			return _items;
		}
	}
}