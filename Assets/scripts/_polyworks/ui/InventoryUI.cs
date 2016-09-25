using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Polyworks {
	public class InventoryUI : UIController {
		#region public members
		public GameObject inventoryItem; 

		public int numColumns = 6;
		public int numRows = 2;

		public string[] ignoredItems;
		#endregion

		#region private members
		private const float startX = -430f;
		private const float startY = 85f;

		private ArrayList _items;
		private int _itemsIndex;

		private float _width; 
		private float _height; 

		private float _horizontal;
		private float _vertical;

		private int _currentColumn;
		private int _currentRow;
		private int _currentItemIndex; 
		private int _previousItemIndex;

		private bool _isInspectingItem = false;
		private bool _isBuilt = false;
		private bool _isDirty = false;

		private Inventory _playerInventory; 

		private InventoryItemUI _selectedInventoryItemUI = null; 
		#endregion

		#region handlers
		public void OnInventoryAdded(string name, int count, bool isPlayerInventory) {
			// Debug.Log ("InventoryUI/OnInventoryAdded, name = " + name);
			if (isPlayerInventory) {
				bool isIgnored = false;
				for (int i = 0; i < ignoredItems.Length; i++) {
					if (name == ignoredItems [i]) {
						return;
					}
				}
				_resetItems();
			}
		}

		public void OnInventoryRemoved(string name, int count) {
			// Debug.Log ("InventoryUI/OnInventoryRemoved, name = " + name);
			_resetItems();
		}

		public void OnCloseInventoryUI() {
			_reset();
		}

		public void OnInspectItem(bool isInspecting, string item) {
			_isInspectingItem = isInspecting;
		}
		#endregion

		#region public methods
		public override void Init() {
			// Debug.Log ("InventoryUI/Init");
			base.Init ();
			_itemsIndex = -1;
			_items = new ArrayList();

			_buildUI();

			var ec = EventCenter.Instance;
			ec.OnInventoryAdded += OnInventoryAdded;
			ec.OnInventoryRemoved += OnInventoryRemoved;
			ec.OnInspectItem += OnInspectItem;
			ec.OnCloseInventoryUI += OnCloseInventoryUI;
		}

		public void InitInventory(Inventory playerInventory) {
			_playerInventory = playerInventory;
			_resetItems ();

		}

		public override void SetActive(bool isActive) {
			base.SetActive (isActive);

			if (_itemsIndex > -1) {
				var item = _items [_currentItemIndex] as InventoryItemUI;
				item.SetFocus (true);
			}
		}
		#endregion

		private void Awake() {
			Init ();
		}
			
		private void _reset() {
			if (_selectedInventoryItemUI != null) {
				_selectedInventoryItemUI.Deselect ();
				_selectedInventoryItemUI = null;
			}
			var item = _items[_currentItemIndex] as InventoryItemUI;
			item.SetFocus (false);

			_currentItemIndex = 0;
			_currentColumn = 0;
			_currentRow = 0;
			_currentItemIndex = 0; 
			_previousItemIndex = 0;
		}

		#region build
		private void _setItem(string name) {
			if(_itemsIndex == (numColumns * numRows) - 1) {
				return;
			}
			_itemsIndex++;

			CollectableItemData itemData = _playerInventory.Get(name);
			InventoryItemUI itemUI = _items[_itemsIndex] as InventoryItemUI;

			itemUI.name = name;
			itemUI.SetName(itemData.displayName);
			itemUI.SetCount(itemData.count);

			if(itemData.thumbnail != "") {
				 Debug.Log("Inventory/_setItem, itemData.thumbnail = " + itemData.thumbnail);
				GameObject itemObj = (GameObject)Instantiate (Resources.Load (itemData.thumbnail, typeof(GameObject)), transform.position, transform.rotation);
				Image thumbnail = itemObj.GetComponent<Image>();
				itemUI.SetThumbnail(thumbnail.sprite);
			}
			if(_itemsIndex == 0) {
				itemUI.SetFocus(true);
			}
		}

		private void _resetItems() {
			InventoryItemUI itemUI;

			_itemsIndex = -1;
			_previousItemIndex = _currentItemIndex = 0;

			for(int i = 0; i < _items.Count; i++) {
				itemUI = _items[i] as InventoryItemUI;
				itemUI.Reset();
			}

			int total = numColumns * numRows;
			int count = 0;

			Hashtable items = _playerInventory.GetAll();
			foreach(CollectableItemData itemData in items.Values) {
				if (count < total) {
					_setItem (itemData.name);
				}
				count++;
			}
		}

		private void _buildUI() {
			RectTransform containerRectTransform = canvas.GetComponent<RectTransform>();
			int total = numColumns * numRows;
			int row = 0;
			int col = 0;

			_width = containerRectTransform.rect.width / numColumns;
			_height = containerRectTransform.rect.height / numRows;

			for(int i = 0; i < total; i++) {
				float x = startX + (_width * col);
				float y = startY + -(_height * row);

				string name = "item" + i;
				var item = (GameObject) Instantiate(inventoryItem);

				item.transform.SetParent(canvas.gameObject.transform, false);

				InventoryItemUI itemUI = item.GetComponent<InventoryItemUI>();
				RectTransform rect = item.GetComponent<RectTransform>();

				item.name = name;

				rect.localPosition = new Vector3(x, y, 0);
				_items.Add(itemUI);

				col++;
				if(col % numColumns == 0) {
					row++;
					col = 0;
				}
			}
			_isBuilt = true;
		}
		#endregion

		#region update
		private void FixedUpdate() {
			if (canvas.enabled) {
				_checkInput ();
			}
		}

		private void _checkInput() {
			if (cancel) {
				if (!_isInspectingItem) {
					if (_selectedInventoryItemUI == null) {
						EventCenter.Instance.CloseInventoryUI ();
					} else {
						_selectedInventoryItemUI.Deselect ();
						_selectedInventoryItemUI = null;
					}
				}
				cancel = false;
			} else {
				if(_itemsIndex > -1 && !_isInspectingItem) {
					if(confirm) {
						if(_selectedInventoryItemUI == null) {
							if (_currentItemIndex <= _itemsIndex) {
								_selectedInventoryItemUI = _items [_currentItemIndex] as InventoryItemUI;
								if (_selectedInventoryItemUI != null) {
									_selectedInventoryItemUI.Select ();
								}
							}
						} else {
							if(_selectedInventoryItemUI != null) {
								_selectedInventoryItemUI.SelectControlButton();
							}
						}
						confirm = false;
					} else if(_selectedInventoryItemUI != null) {
						if(up) {
							if(_selectedInventoryItemUI != null) {
								_selectedInventoryItemUI.IncrementControlButtonFocus(false);
							}
						} else if(down) {
							if(_selectedInventoryItemUI != null) {
								_selectedInventoryItemUI.IncrementControlButtonFocus(true);
							}
						}
					} else if(_selectedInventoryItemUI == null) {
						_horizontal = 0;
						_vertical = 0;
						if(up) {
							_vertical = 1;
						} else if(down) {
							_vertical = -1;
						} else if(left) {
							_horizontal = -1;
						} else if(right) {
							_horizontal = 1;
						}

						if(_horizontal != 0 || _vertical != 0) {
							if(_horizontal != 0) {
								_calculateCol(_horizontal);
							} else if(_vertical != 0) {
								_calculateRow(_vertical);
							}
							_currentItemIndex = (_currentRow * numColumns) + _currentColumn;
							var item = _items[_currentItemIndex] as InventoryItemUI;

							if(item != null) {
								item.SetFocus(true);
							}
							var prevItem = _items[_previousItemIndex] as InventoryItemUI;
							if(prevItem != null) {
								prevItem.SetFocus(false);
							}
							_previousItemIndex = _currentItemIndex;
						}
					}
				}
			}
		}
		#endregion

		#region ui nav
		private void _calculateCol(float horizontal) {
			if(horizontal < 0) {
				_decrementCol(true);
			} else  {
				_incrementCol(true);
			}
		}

		private void _calculateRow(float vertical) {
			if(vertical > 0) {
				_decrementRow(true);
			} else if(vertical < 0) {
				_incrementRow(true);
			}
		}

		private void _incrementCol(bool isCalcCalled) {
			if(_currentColumn < (numColumns - 1)) {
				_currentColumn++;
			} else {
				_currentColumn = 0;
				if(isCalcCalled) {
					_incrementRow(false);
				}
			}
		}

		private void _decrementCol(bool isCalcCalled) {
			if(_currentColumn > 0) {
				_currentColumn--;
			} else {
				_currentColumn = (numColumns - 1);
				if(isCalcCalled) {
					_decrementRow(false);
				}
			}
		}

		private void _incrementRow(bool isCalcCalled) {
			if(_currentRow < (numRows - 1)) {
				_currentRow++;
			} else {
				_currentRow = 0;
				if(isCalcCalled) {
					_incrementCol(false);
				}
			}
		}

		private void _decrementRow(bool isCalcCalled) {
			if(_currentRow > 0) {
				_currentRow--;
			} else {
				_currentRow = (numRows - 1);
				if(isCalcCalled) {
					_decrementCol(false);
				}
			}
		}

		private bool _increment(int counter, int max, bool isCalcCalled = false) {
			if(counter < (max - 1)) {
				counter++;
				return false;
			} else {
				counter = 0;
				return true;
			}
		}

		private bool _decrement(int counter, int max, bool isCalcCalled = false) {
			if(counter > 0) {
				counter--;
				return false;
			} else {
				counter = (max - 1);
				return true;
			}
		}
		#endregion

		private void OnDestroy() {
			// Debug.Log ("Inventory/OnDestroy");
			var ec = EventCenter.Instance;
			if (ec != null) {
				ec.OnInventoryAdded -= OnInventoryAdded;
				ec.OnInventoryRemoved -= OnInventoryRemoved;
				ec.OnInspectItem -= OnInspectItem;
				ec.OnCloseInventoryUI -= OnCloseInventoryUI;
			}
		}

	}
}