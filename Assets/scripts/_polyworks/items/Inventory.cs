using UnityEngine;
using System.Collections;

namespace Polyworks {

	public class Inventory : MonoBehaviour
	{
		private Hashtable _items;
		private EventCenter _eventCenter; 

		private bool _isPlayerInventory; 

		public void Init(Hashtable items = null, bool isPlayerInventory = false) {
			// Debug.Log ("Inventory/Init, items.Count = " + items.Count);
			_isPlayerInventory = isPlayerInventory;

			if (items != null && items.Count > 0) {
				_items = items;
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
				_eventCenter.AddNote (itemData.displayName + " Added");
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
			ItemData data = Get (name);			
			if(data == null) {
				return; 
			}
			data.isUsable = ItemUtils.GetIsUsable (data);
			if (data.isUsable) {
				CollectableItem item = _pluck(name); 

				item.Use ();

				if (item.data.isDestroyedOnUse) {
					Destroy (item.gameObject);
				} else {
					_initProximityAgent (item);
				}
			} else {
				_eventCenter.AddNote (data.displayName + " Can not be used here");
			}
			_eventCenter.CloseInventoryUI ();
		}

		public virtual void Drop(string name) {
			// Debug.Log("Inventory/Drop, name = " + name);
			CollectableItem item = _pluck(name); 
			
			if(item == null) {
				return;
			}
			
			_initProximityAgent(item);
			_eventCenter.CloseInventoryUI ();
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

		private void OnDestroy() {
			// Debug.Log ("Inventory/OnDestroy");
		}
		
		private CollectableItem _pluck(string name) {
			ItemData data = Remove (name);
			if (data == null) {
				return null;
			}
			string prefab = data.prefabPath + data.itemName;
			GameObject itemObj = (GameObject) Instantiate (Resources.Load (prefab, typeof(GameObject)), transform.position, transform.rotation);
			CollectableItem item = itemObj.GetComponent<CollectableItem> ();
			item.data = data;
			item.data.isCollected = false;

			return item;
		}
		
		private void _initProximityAgent(CollectableItem item) {
			ProximityAgent pc = item.gameObject.GetComponent<ProximityAgent>();
			if(pc == null) {
				return;
			}
			pc.Init();
		}
	}
}