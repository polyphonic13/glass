using UnityEngine;
using System.Collections;

namespace Polyworks {

	public class Inventory : MonoBehaviour
	{
		private Hashtable _items;

		public void Init(Hashtable items = null) {
			if (items != null && items.Count > 0) {
				_items = items as Hashtable;
			} else if (_items == null) {
				_items = new Hashtable ();
			}
		}

		public virtual void Add(ItemData item) {
			if (!Contains (item.itemName)) {
				InsertItem (item);
			}
			ItemData itemData = Get (item.itemName) as ItemData;
			itemData.count++;
			EventCenter.Instance.InventoryAdded (itemData.itemName, itemData.count);
			EventCenter.Instance.AddNote (itemData.itemName + " Added");
		}

		public virtual void InsertItem(ItemData item) {
			// Debug.Log ("Inventory/InsertItem, item.itemName = " + item.itemName + ", _items = " + _items);
			_items.Add (item.itemName, item);
			EventCenter.Instance.InventoryAdded (item.itemName, item.count);
		}

		public virtual GameObject Remove(string name) {
			if (!Contains (name)) {
				return null;
			} else {
				var data = _items [name] as ItemData;

				if (data != null && data.count > 0) {
					data.count--;

					if (data.count == 0) {
						_items.Remove (name);
					}
					if (data.isDroppable && data.prefabName != null) {
						GameObject itemObject = (GameObject) Instantiate (Resources.Load(data.prefabName, typeof(GameObject)), transform.position, transform.rotation);
						return itemObject;
					} else {
						return null;
					}
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
			// Debug.Log ("Inventory/GetAll, _items.Count = " + _items.Count);
			return _items;
		}
	}
}