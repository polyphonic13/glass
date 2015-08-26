using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

	[SerializeField] private int _maxItems = 15;

	private Hashtable _items;

	private static Inventory _instance;
	private Inventory() {
		_items = new Hashtable();
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
			item.transform.position = this.transform.position;
			item.transform.parent = this.transform;
			EventCenter.Instance.AddInventory(item.name);
		} else {
			EventCenter.Instance.AddNote("No more room for: " + displayName);
			isAdded = false;
		}
		return isAdded;
	}

	public void RemoveItem(string key) {
		Debug.Log("Inventory/RemoveItem, key = " + key);
		if(HasItem(key)) {
			var item = _items[key] as CollectableItem;
			item.IsCollected = false;
			item.transform.position = Camera.main.transform.position;
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
