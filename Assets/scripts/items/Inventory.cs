using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

	[SerializeField] private int _maxItems = 15;

	private Transform _backpack;

	private Hashtable _items;

	private static Inventory _instance;
	private Inventory() {}

	private void Awake() {
		_items = new Hashtable();
		EventCenter.Instance.OnInspectItem += OnInspectItem;
		_backpack = GameObject.Find ("backpack").gameObject.transform;
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
		var displayName = item.GetName();
		var isAdded = true;
		// Debug.Log("Inventory/AddItem, item = " + displayName);
		if(HasItem(item.name)) {
			// increment count of item type
		} else if(_items.Count < _maxItems) {
			EventCenter.Instance.AddNote(displayName + " Added to inventory");
			_items.Add(item.name, item);
			item.Collect(_backpack);
//			item.Store();
//			item.transform.position = this.transform.position;
//		 	item.transform.parent = this.transform;
			EventCenter.Instance.AddInventory(item.name);
		} else {
			EventCenter.Instance.AddNote("No more room for: " + displayName);
			isAdded = false;
		}
		return isAdded;
	}

	public void OnInspectItem(bool isInspecting, string key) {
		if (HasItem (key)) {
			var item = _items[key] as CollectableItem;
			if (isInspecting) {
				ItemInspector.Instance.AddTarget (item.transform, item.GetName(), item.description);
			} else {
				ItemInspector.Instance.RemoveTarget ();
			}
			item.IsInspected = !item.IsInspected;
		}
	}

	public void OnUseItem(string key) {
		if (HasItem (key)) {
			var item = _items[key] as CollectableItem;

		}
	}

	public void RemoveItem(string key) {
//		Debug.Log("Inventory/RemoveItem, key = " + key);
		if(HasItem(key)) {
			var item = _items[key] as CollectableItem;
//			item.IsCollected = false;
			item.Drop();
			_items.Remove(key);
			EventCenter.Instance.RemoveInventory(item.name);
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
