using UnityEngine;
using System.Collections;

namespace Polyworks {

	public class Inventory : MonoBehaviour
	{
		private Hashtable _items;

		private static Inventory _instance;
		private Inventory() {}

		public void Init() {
			if (GameControl.Instance.inventoryItems != null) {
				_items = GameControl.Instance.inventoryItems as Hashtable;
				if (_items.Count > 0) {
					foreach(ItemData item in _items.Values) {
						EventCenter.Instance.AddInventory(item.ItemName);
					}
				}
			} else {
				_items = new Hashtable ();
			}
		}

		public virtual void Add(ItemData item) {
			string message;

			item.IsCollected = true;
			_items.Add (item.ItemName, item);

			message = item.ItemName + " Collected";

			EventCenter.Instance.AddNote(message);
			EventCenter.Instance.AddInventory(item.ItemName);
		}

		public virtual void Remove(string name) {
			if (Contains (name)) {
				var data = _items [name] as ItemData;
				_items.Remove (name);

				if (data.IsDroppable && data.ItemObject != null) {
					GameObject itemObject = (GameObject) Instantiate (data.ItemObject, transform.position, transform.rotation);
				}
			}
		}

		public bool Contains(string key) {
			return(_items.Contains(key)) ? true : false;
		}
	}
}