using UnityEngine;
using System.Collections;
using UnitySampleAssets.CrossPlatformInput;

public class InventoryUI : MonoBehaviour {

	[SerializeField] private GameObject _inventoryItem; 

	[SerializeField] private int _numColumns = 5;
	[SerializeField] private int _numRows = 3;

	private const float START_X = -430f;
	private const float START_Y = 85f;

	private ArrayList _items;
	private int _occupiedItems;

	private float _width; 
	private float _height; 

	private float _horizontal;
	private float _vertical;

	private int _currentColumn;
	private int _currentRow;
	private int _currentItemIndex; 
	private int _previousItemIndex;
	private float _previousTime;

	private bool _isSelectingItem;
	private InventoryItemUI _selectedInventoryItemUI; 

	private Canvas _canvas;

    public void OnInventoryAdded(string itemName) {
    	_setItem(itemName);	
    }

	public void OnInventoryRemoved(string itemName) {
		_resetItems(itemName);
	}
	
	private void Awake() {
		_items = new ArrayList();
		_canvas = gameObject.transform.parent.GetComponent<Canvas>();
		_buildUI();

		var ec = EventCenter.Instance;
		ec.OnInventoryAdded += OnInventoryAdded;
		ec.OnInventoryRemoved += OnInventoryRemoved;
	}

	private void Update() {
		if(_canvas.enabled) {
			_checkInput();
        }
    }

    private void _setItem(string itemName) {
    	if(_occupiedItems == (_numColumns * _numRows)) {
    		return;
    	}
    	var item = Inventory.Instance.GetItem(itemName);
    	var itemUI = _items[_occupiedItems] as InventoryItemUI;

		itemUI.name = itemName;
		itemUI.SetName(item.GetName());
   		itemUI.SetCount(1);
    	if(item.Thumbnail != null) {
	    	itemUI.SetThumbnail(item.Thumbnail);
    	}
    	if(_occupiedItems == 0) {
    		itemUI.SetFocus(true);
    	}
    	_occupiedItems++;
    }

	private void _resetItems(string itemName) {
		InventoryItemUI itemUI;
		int i;
		
		_occupiedItems = 0;
		
		for(i = 0; i < _items.Count; i++) {
			itemUI = _items[i] as InventoryItemUI;
			
			if(itemUI.name != itemName) {
				_items.RemoveAt(i);
			}
			itemUI.Reset();
		}
		
		// have to loop again now that _items order changed
		for(i = 0; i < _items.Count; i++) {
			itemUI = _items[i] as InventoryItemUI;
			_setItem(itemUI.name);
		}
	}
	
    private void _buildUI() {
		RectTransform containerRectTransform = GetComponent<RectTransform>();
		int total = _numColumns * _numRows;
        int row = 0;
		int col = 0;

		Hashtable hash = Inventory.Instance.GetAll();
		var inventory = new ArrayList(hash.Values);

		_width = containerRectTransform.rect.width / _numColumns;
		_height = containerRectTransform.rect.height / _numRows;
		_previousTime = Time.realtimeSinceStartup;

		for(int i = 0; i < total; i++) {
			float x = START_X + (_width * col);
			float y = START_Y + -(_height * row);

			string itemName = "item" + i;
			var item = Instantiate(_inventoryItem);
	
			item.transform.SetParent(gameObject.transform, false);

			InventoryItemUI itemUI = item.GetComponent<InventoryItemUI>();
			RectTransform rect = item.GetComponent<RectTransform>();

			itemUI.name = item.name = itemName;

			rect.localPosition = new Vector3(x, y, 0);

			_items.Add(itemUI);

			col++;
			if(col % _numColumns == 0) {
				row++;
				col = 0;
			}
		}
    }

    private void _checkInput() {
    	if(_occupiedItems > 0) {
    		if(CrossPlatformInputManager.GetButtonDown("Fire1")) {
				if(!_isSelectingItem) {
					_selectedInventoryItemUI = _items[_currentItemIndex] as InventoryItemUI;
					if(_selectedInventoryItemUI != null) {
						_selectedInventoryItemUI.Select();
					}
				} else {
					if(_selectedInventoryItemUI != null) {
						_selectedInventoryItemUI.SelectControlButton();
					}
				}
			} else if(_isSelectingItem) {
				if(CrossPlatformInputManager.GetButtonDown("Cancel")) {
					if(_selectedInventoryItemUI != null) {
						_selectedInventoryItemUI.Deselect();
					}
					_isSelectingItem = false;
				} else if(DelayedAxisInput.Check("vertical", _horizontal, _vertical)) {
					var setFocus = true;
					if(_vertical > 0) {
						setFocus = false;
					}
					if(_selectedInventoryItemUI != null) {
						_selectedInventoryItemUI.SetControlButtonFocus(setFocus);
					}
				}
			} else if(!_isSelectingItem) {
				if(DelayedAxisInput.Check("both", _horizontal, _vertical)) {
					if(_horizontal != 0) {
						_calculateAxis(_horizontal, _currentRow, _numRows, _currentColumn, _numColumns);
					} else if(_vertical != 0) {
						_calculateAxis(_vertical, _currentColumn, _numColumns, _currentRow, _numRows);
					}
					_currentItemIndex = (_currentRow * _numColumns) + _currentColumn;
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

	private void _calculateAxis(float axisValue, int axisCounter, int axisMax, int oppositeCounter, int oppositeMax) {
		if(axisValue < 0) {
			if(_decrement(axisCounter, axisMax, true)) {
				_decrement(oppositeCounter, oppositeMax);
			}
		} else {
			if(_increment(axisCounter, axisMax, true)) {
				_increment(oppositeCounter, oppositeMax);
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
	
 }
