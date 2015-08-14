using UnityEngine;
using System.Collections;

public class InventoryManager {
	
	public bool ShowInventory { get; set; }
	public bool ShowDetail { get; set; }

	public string EquippedItem { get; set; }
	public bool IsItemEquipped { get; set; }
	public bool IsHouseKeepingNeeded { get; set; }

	private const float DETAIL_IMG_WIDTH_HEIGHT = 500;
	private const float ICON_WIDTH_HEIGHT = 100;
	private const int ITEMS_WIDTH = 5;
	private const int ITEM_NAME_HEIGHT = 20;
	private const int ITEM_SPACING = 20;

	private Hashtable _itemsHash;
	
	private GUIStyle _style; 
	private CollectableItem _detailInventoryItem;

	private Vector2 _offset; 

	private string _itemToDelete = ""; 
	
	public void Init(GUIStyle style) {
		ShowInventory = false;
		ShowDetail = false;
		IsHouseKeepingNeeded = false;
		_style = style;
		_itemsHash = new Hashtable();
		_offset = new Vector2(10, 10);
		_resetEquippedItem();
	}
	
	public void AddItem(CollectableItem item) {
//		Debug.Log("_items manager/AddItem, item = " + item.name + ", description = " + item.description);
		EventCenter.Instance.AddNote(item.ItemName + " Added to inventory");
		_itemsHash.Add(item.name, item);
	}
	
	public bool HasItem(string name) {
		return(_itemsHash.Contains(name)) ? true : false;
	}
	
	public string GetItemName(string key) {
		if(_itemsHash.Contains(key)) {
			var item = _itemsHash[key] as CollectableItem;
			return item.ItemName;
		} else {
			return "";
		}
	}

	public void DrawSummary() {
		EventCenter.Instance.EnablePlayer(false);
		int j;
		int k;
//		InventoryItem currentInventoryItem;
		CollectableItem currentInventoryItem;
		Rect currentRect;
		DrawBackground("Inventory");

		int i = 0;
		Hashtable _tempItems = _itemsHash;
		foreach(DictionaryEntry key in _tempItems) {
			currentInventoryItem = key.Value as CollectableItem;
			j = i / ITEMS_WIDTH;
			k = i % ITEMS_WIDTH;
			currentRect =(new Rect(_offset.x + k *(ICON_WIDTH_HEIGHT + ITEM_SPACING), _offset.y + j *(ICON_WIDTH_HEIGHT + ITEM_SPACING), ICON_WIDTH_HEIGHT, ICON_WIDTH_HEIGHT));
			if(currentInventoryItem == null) {          
					// GUI.DrawTexture(currentRect, _emptySlot);
			} else {
				// Debug.Log("about to Draw texture for " + currentInventoryItem.iconTexture + ", currentRect = " + currentRect);
				GUI.Box(new Rect(currentRect.x, currentRect.y, ICON_WIDTH_HEIGHT, ITEM_NAME_HEIGHT), currentInventoryItem.ItemName);
				GUI.DrawTexture(new Rect(currentRect.x, currentRect.y + ITEM_NAME_HEIGHT, currentRect.width, currentRect.height), currentInventoryItem.iconTexture);
				var controlBtnRect = new Rect(currentRect.x,(currentRect.y + ICON_WIDTH_HEIGHT + 5 + ITEM_NAME_HEIGHT), ICON_WIDTH_HEIGHT / 2, 20);
				if(GUI.Button(controlBtnRect, "Detail")) {
					_detailInventoryItem = currentInventoryItem as CollectableItem;
					ShowInventory = false;
					ShowDetail = true;
				}

				GUI.enabled = currentInventoryItem.IsEquipable;
				if(!currentInventoryItem.IsEquipped) {
						if(GUI.Button(new Rect(controlBtnRect.x +(ICON_WIDTH_HEIGHT / 2), controlBtnRect.y, controlBtnRect.width, controlBtnRect.height), "Equip")) {
							_equipAndClose(currentInventoryItem.name);
						}
				} else {
						if(GUI.Button(new Rect(controlBtnRect.x +(ICON_WIDTH_HEIGHT / 2), controlBtnRect.y, controlBtnRect.width, controlBtnRect.height), "Store")) {
							_equipAndClose(currentInventoryItem.name);
						}
				}
				GUI.enabled = true;
				if(currentInventoryItem.IsDroppable) {
					if(!currentInventoryItem.IsEquipped) {
						GUI.enabled = false;
					}
					if(GUI.Button(new Rect(currentRect.x,(currentRect.y + ICON_WIDTH_HEIGHT + 25 + ITEM_NAME_HEIGHT), ICON_WIDTH_HEIGHT, 20), "Drop")) {
						_dropAndClose(currentInventoryItem.name);
					}

				}
				GUI.enabled = true;
			}
			i++;
		}
	}
	
	public void DrawDetail() {
		EventCenter.Instance.EnablePlayer(false);
//		Debug.Log("DrawDetail = " + ShowDetail + ", _detailInventoryItem = " + _detailInventoryItem);
		if(_detailInventoryItem != null) {
			var detailImgLeft = Screen.width / 2 - DETAIL_IMG_WIDTH_HEIGHT / 2;
			var detailImgTop = Screen.height / 2 - DETAIL_IMG_WIDTH_HEIGHT / 2;
			var detailRect = new Rect(detailImgLeft, detailImgTop, DETAIL_IMG_WIDTH_HEIGHT + 10, DETAIL_IMG_WIDTH_HEIGHT + 50);
			DrawBackground("Item detail: " + _detailInventoryItem.ItemName);
			// Debug.Log("building detail of: " + _detailInventoryItem.name);
			GUI.Box(detailRect, _detailInventoryItem.description);
			GUI.DrawTexture(new Rect(detailImgLeft + 5, detailImgTop + 45, DETAIL_IMG_WIDTH_HEIGHT, DETAIL_IMG_WIDTH_HEIGHT), _detailInventoryItem.iconTexture);
			if(GUI.Button(new Rect(detailImgLeft - 100, 75, 100, 20), "back")) {
				_detailInventoryItem = null;
				ShowDetail = false;
				ShowInventory = true;
			}
		} else {
			Debug.Log("ERROR: _detailInventoryItem is null");
			ShowDetail = false;
			ShowInventory = false;
		}
	}
	
	public void DropItem() {
		var item = _itemsHash[EquippedItem] as CollectableItem;
		item.Drop();
		_itemToDelete = EquippedItem;
		IsHouseKeepingNeeded = true;
		_resetEquippedItem();
	}
	
	public void HouseKeeping() {
		Debug.Log("InventoryManager/HouseKeeping, _itemToDelete = " + _itemToDelete);
		if(_itemToDelete != "") {
			_itemsHash.Remove(_itemToDelete);
			_itemToDelete = "";
		}
		IsHouseKeepingNeeded = false;
	}
	
	public void DrawBackground(string title) {
		GUI.Box(new Rect(5, 5, Screen.width - 10, Screen.height - 10), title /*, _style */);
	}
	
	public void Close() {
		ShowInventory = false;
		ShowDetail = false;
		EventCenter.Instance.EnablePlayer(true);
	}

	private void _equipAndClose(string ItemName) {
		EventCenter.Instance.EquipItem(ItemName);
		EquippedItem = ItemName;
		IsItemEquipped = true;
		Close();
	}
	
	private void _resetEquippedItem() {
		EquippedItem = "";
		IsItemEquipped = false;
	}
	
	private void _dropAndClose(string ItemName) {
		DropItem();
		Close();
	}


}
