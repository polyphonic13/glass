using UnityEngine;
using System.Collections;

public class InventoryManager {
	
	public bool showInventory { get; set; }
	public bool showDetail { get; set; }

	public string equippedItem { get; set; }
	public bool isItemEquipped { get; set; }
	
	public bool houseKeepingNeeded { get; set; }

	private const float DETAIL_IMG_WIDTH_HEIGHT = 500;
	private const float ICON_WIDTH_HEIGHT = 100;
	private const int ITEMS_WIDTH = 5;
	private const int ITEM_NAME_HEIGHT = 20;
	private const int ITEM_SPACING = 20;

	private Hashtable _itemsHash;
	
	private Vector2 _offset; 
	private Texture _emptySlot; 
	private GUIStyle _style; 
	private CollectableItem _detailInventoryItem = null;
	private CollectableItem _equipedItem = null;
	
	private string _itemToDelete = ""; 
	
	public void Init(GUIStyle style) {
		showInventory = false;
		showDetail = false;
		houseKeepingNeeded = false;
		_style = style;
		_itemsHash = new Hashtable();
		_offset = new Vector2(10, 10);
		_resetEquippedItem();
	}
	
	public void addItem(CollectableItem item) {
//		Debug.Log("_items manager/addItem, item = " + item.name + ", description = " + item.description);
		EventCenter.Instance.addNote(item.itemName + " added to inventory");
		_itemsHash.Add(item.name, item);
	}
	
	public bool hasItem(string name) {
		bool found = false;
		if(_itemsHash.Contains(name)) {
			return true;
		} else {
			return false;
		}
	}
	
	public string getItemName(string key) {
		if(_itemsHash.Contains(key)) {
			var item = _itemsHash[key] as CollectableItem;
			return item.itemName;
		} else { 
			return "";
		}
	}

	public void drawSummary() {
		EventCenter.Instance.enablePlayer(false);
		int j;
		int k;
//		InventoryItem currentInventoryItem = null;
		CollectableItem currentInventoryItem = null;
		Rect currentRect;
		drawBackground ("Inventory");

		int i = 0;
		Hashtable _tempItems = _itemsHash;
		foreach (DictionaryEntry key in _tempItems) {
			currentInventoryItem = key.Value as CollectableItem;
			j = i / ITEMS_WIDTH;
			k = i % ITEMS_WIDTH;
			currentRect = (new Rect (_offset.x + k * (ICON_WIDTH_HEIGHT + ITEM_SPACING), _offset.y + j * (ICON_WIDTH_HEIGHT + ITEM_SPACING), ICON_WIDTH_HEIGHT, ICON_WIDTH_HEIGHT));
			if (currentInventoryItem == null) {          
					GUI.DrawTexture (currentRect, _emptySlot);
			} else {
				// Debug.Log("about to draw texture for " + currentInventoryItem.iconTexture + ", currentRect = " + currentRect);
				GUI.Box (new Rect (currentRect.x, currentRect.y, ICON_WIDTH_HEIGHT, ITEM_NAME_HEIGHT), currentInventoryItem.itemName);
				GUI.DrawTexture (new Rect (currentRect.x, currentRect.y + ITEM_NAME_HEIGHT, currentRect.width, currentRect.height), currentInventoryItem.iconTexture);
				Rect controlBtnRect = new Rect (currentRect.x, (currentRect.y + ICON_WIDTH_HEIGHT + 5 + ITEM_NAME_HEIGHT), ICON_WIDTH_HEIGHT / 2, 20);
				if (GUI.Button (controlBtnRect, "Detail")) {
					_detailInventoryItem = currentInventoryItem as CollectableItem;
					showInventory = false;
					showDetail = true;
				}

				GUI.enabled = currentInventoryItem.isEquipable;
				if (!currentInventoryItem.isEquipped) {
						if (GUI.Button (new Rect (controlBtnRect.x + (ICON_WIDTH_HEIGHT / 2), controlBtnRect.y, controlBtnRect.width, controlBtnRect.height), "Equip")) {
							_equipAndClose (currentInventoryItem.name);
						}
				} else {
						if (GUI.Button (new Rect (controlBtnRect.x + (ICON_WIDTH_HEIGHT / 2), controlBtnRect.y, controlBtnRect.width, controlBtnRect.height), "Store")) {
							_equipAndClose (currentInventoryItem.name);
						}
				}
				GUI.enabled = true;
				if (currentInventoryItem.isDroppable) {
					if(!currentInventoryItem.isEquipped) {
						GUI.enabled = false;
					}
					if (GUI.Button (new Rect (currentRect.x, (currentRect.y + ICON_WIDTH_HEIGHT + 25 + ITEM_NAME_HEIGHT), ICON_WIDTH_HEIGHT, 20), "Drop")) {
						_dropAndClose(currentInventoryItem.name);
					}

				}
				GUI.enabled = true;
			}
			i++;
		}
	}
	
	public void drawDetail() {
		EventCenter.Instance.enablePlayer(false);
//		Debug.Log("drawDetail = " + showDetail + ", _detailInventoryItem = " + _detailInventoryItem);
		if(_detailInventoryItem != null) {
			var detailImgLeft = Screen.width / 2 - DETAIL_IMG_WIDTH_HEIGHT / 2;
			var detailImgTop = Screen.height / 2 - DETAIL_IMG_WIDTH_HEIGHT / 2;
			Rect detailRect = new Rect(detailImgLeft, detailImgTop, DETAIL_IMG_WIDTH_HEIGHT + 10, DETAIL_IMG_WIDTH_HEIGHT + 50);
			drawBackground("Item detail: " + _detailInventoryItem.itemName);
			// Debug.Log("building detail of: " + _detailInventoryItem.name);
			GUI.Box(detailRect, _detailInventoryItem.description);
			GUI.DrawTexture(new Rect(detailImgLeft + 5, detailImgTop + 45, DETAIL_IMG_WIDTH_HEIGHT, DETAIL_IMG_WIDTH_HEIGHT), _detailInventoryItem.iconTexture);
			if(GUI.Button(new Rect(detailImgLeft - 100, 75, 100, 20), "back")) {
				_detailInventoryItem = null;
				showDetail = false;
				showInventory = true;
			}
		} else {
			Debug.Log("ERROR: _detailInventoryItem is null");
			showDetail = false;
			showInventory = false;
		}
	}
	
	public void dropItem() {
		CollectableItem item = _itemsHash[equippedItem] as CollectableItem;
		item.drop();
		_itemToDelete = equippedItem;
		houseKeepingNeeded = true;
		_resetEquippedItem();
	}
	
	public void houseKeeping() {
		Debug.Log("InventoryManager/houseKeeping, _itemToDelete = " + _itemToDelete);
		if(_itemToDelete != "") {
			_itemsHash.Remove(_itemToDelete);
			_itemToDelete = "";
		}
		houseKeepingNeeded = false;
	}
	
	public void drawBackground(string title) {
		GUI.Box(new Rect(5, 5, Screen.width - 10, Screen.height - 10), title /*, _style */);
	}
	
	public void close() {
		showInventory = false;
		showDetail = false;
		EventCenter.Instance.enablePlayer(true);
	}

	private void _equipAndClose(string itemName) {
		EventCenter.Instance.equipItem(itemName);
		equippedItem = itemName;
		isItemEquipped = true;
		close();
	}
	
	private void _resetEquippedItem() {
		equippedItem = "";
		isItemEquipped = false;
	}
	
	private void _dropAndClose(string itemName) {
		dropItem();
		close();
	}


}
