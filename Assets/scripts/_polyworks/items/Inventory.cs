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

		public virtual void Add(CollectableItemData data, bool increment = true) {
			if (!Contains (data.itemName)) {
				_items.Add (data.itemName, data);
			}
//			CollectableItemData itemData = Get (data.itemName) as CollectableItemData;
			if (increment) {
				data.count++;
			}
			if (_isPlayerInventory) {
				_eventCenter.InventoryAdded (data.itemName, data.count, _isPlayerInventory);
				_eventCenter.AddNote (data.displayName + " Added");
			}
		}

		public virtual CollectableItemData Remove(string name) {
			if (!Contains (name)) {
				return null;
			} else {
				var data = _items [name] as CollectableItemData;

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
			CollectableItemData data = Get (name);			
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

		public virtual CollectableItemData Get(string name) {
			if (Contains (name)) {
				return _items [name] as CollectableItemData;
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
			CollectableItemData data = Remove (name);
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