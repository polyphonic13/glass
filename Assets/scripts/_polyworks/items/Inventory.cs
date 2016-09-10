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
			if (!Contains (data.name)) {
				_items.Add (data.name, data);
			}
//			CollectableItemData itemData = Get (data.name) as CollectableItemData;
			if (increment) {
				data.count++;
			}
			if (_isPlayerInventory) {
				_eventCenter.InventoryAdded (data.name, data.count, _isPlayerInventory);
				_eventCenter.AddNote (data.displayName + " added");
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
					_eventCenter.InventoryRemoved (data.name, data.count);
				}
				return data;
			} 
		}

		public virtual void Use(string name) {
			CollectableItemData data = Get (name);	
			Debug.Log ("Inventory/Use, data = " + data);
			if(data == null) {
				return; 
			}
			data.isUsable = ItemUtils.GetIsUsable (data);
			Debug.Log ("is usable = " + data.isUsable);
			if (data.isUsable) {
				Remove (name);
				CollectableItem item = _pluck(data); 
				Debug.Log ("the item is = " + item);
				item.Use ();
//
//				if (item.data.isDestroyedOnUse) {
//					Destroy (item.gameObject);
//				} else {
//					_initDroppedItem (item);
//				}
			} else {
				_eventCenter.AddNote (data.displayName + " Can not be used here");
			}
			_eventCenter.CloseInventoryUI ();
		}

		public virtual void Drop(string name) {
			// Debug.Log("Inventory/Drop, name = " + name);
			CollectableItemData data = Remove(name);
			CollectableItem item = _pluck(data); 
			
			if(item == null) {
				return;
			}
			
			_initDroppedItem(item);
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

		public CollectableItem GetItem(string name) {
			
			return _pluck (Get(name));
		}

		private void Awake() {
			_eventCenter = EventCenter.Instance;
		}

		private void OnDestroy() {
			// Debug.Log ("Inventory/OnDestroy");
		}
		
		private CollectableItem _pluck(CollectableItemData data) {
			if (data == null) {
				return null;
			}
			Debug.Log ("prefabPath = " + data.prefabPath);
			GameObject itemObj = (GameObject) Instantiate (Resources.Load (data.prefabPath, typeof(GameObject)), transform.position, transform.rotation);
			CollectableItem item = itemObj.GetComponent<CollectableItem> ();
			item.data = data;
			item.data.isCollected = false;

			return item;
		}
		
		private void _initDroppedItem(CollectableItem item) {
			ProximityAgent pc = item.gameObject.GetComponent<ProximityAgent>();
			if(pc != null) {
				pc.Init();
			}

			SectionAgent sa = item.gameObject.GetComponent<SectionAgent> ();
			if (sa != null) {
				sa.ToggleEnabled (true);
			}
		}
	}
}