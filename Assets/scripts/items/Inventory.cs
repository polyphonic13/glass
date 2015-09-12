using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

	[SerializeField] private int _maxItems = 15;

	private Transform _backpack;
	private Transform _leftHand;
	private Transform _rightHand;

	private Hashtable _items;

	private static Inventory _instance;
	private Inventory() {}

	private void Awake() {
		_items = new Hashtable();
		Transform player = GameObject.Find ("player").transform;
//		_backpack = player.Find ("backpack");
		_backpack = this.transform;
		_leftHand = player.Find ("player_head_camera/left_hand");
		_rightHand = player.Find ("player_head_camera/right_hand");

		EventCenter.Instance.OnInspectItem += OnInspectItem;
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
		var isAdded = true;
//		 Debug.Log("Inventory/AddItem, item = " + item.name);
		if(HasItem(item.name)) {
			// increment count of item type
		} else if(_items.Count < _maxItems) {
			_items.Add(item.name, item);
			item.Collect(_backpack, _rightHand);
			EventCenter.Instance.AddNote(item.ItemName + " Added to inventory");
//			EventCenter.Instance.AddNote(item.ItemName + " Added to inventory");
			EventCenter.Instance.AddInventory(item.name);
		} else {
			isAdded = false;
			EventCenter.Instance.AddNote("No more room for: " + item.ItemName);
		}
		return isAdded;
	}

	public void OnInspectItem(bool isInspecting, string key) {
		if (HasItem (key)) {
			var item = _items[key] as CollectableItem;
			if (isInspecting) {
				ItemInspector.Instance.AddTarget (item.transform, item.ItemName, item.description);
			} else {
				ItemInspector.Instance.RemoveTarget ();
			}
			item.IsInspected = !item.IsInspected;
		}
	}

	public void UseItem(string key) {
		if (HasItem (key)) {
			var item = _items[key] as CollectableItem;
			if(item.IsUsable) {
				item.Use();
			} else {
				EventCenter.Instance.CloseInventoryUI ();
				EventCenter.Instance.AddNote(item.ItemName + " can not be used here");
			}
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
