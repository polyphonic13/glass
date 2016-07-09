using UnityEngine;
using System.Collections;

namespace Polyworks {

	public class Inventory : MonoBehaviour
	{
		private Hashtable _items;
		private EventCenter _eventCenter; 


		public void Init(Hashtable items = null) {
			if (items != null && items.Count > 0) {
				_items = items as Hashtable;
			} else if (_items == null) {
				_items = new Hashtable ();
			}
		}

		public virtual void Add(ItemData item) {
			if (!Contains (item.itemName)) {
				_items.Add (item.itemName, item);
			}
			ItemData itemData = Get (item.itemName) as ItemData;
			itemData.count++;

			_eventCenter.InventoryAdded (itemData.itemName, itemData.count);
			_eventCenter.AddNote (itemData.itemName + " Added");
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
					return data;

				} else {
					return null;
				}
			} 
		}

		public virtual void Use(string name) {
			ItemData data = Remove (name);
			if (data != null) {
				GameObject itemObj = (GameObject)Instantiate (Resources.Load (data.prefabName, typeof(GameObject)), transform.position, transform.rotation);
				Item item = itemObj.GetComponent<Item> ();
				item.data = data;
				item.Use ();

				if (data.isDestroyedOnUse) {
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
			_eventCenter = EventCenter.Instance;
		}
	}
}