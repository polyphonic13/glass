using UnityEngine;
using System.Collections;
using UnitySampleAssets.CrossPlatformInput;

public class InventoryUI : MonoBehaviour {

	[SerializeField] private GameObject _inventoryItem; 

	[SerializeField] private int _numColumns = 5;
	[SerializeField] private int _numRows = 3;

	private const float START_X = -430f;
	private const float START_Y = 85f;
	private const float INPUT_DELAY = .03f;

	private ArrayList _items;
	private int _occupiedItems;

	private float _width; 
	private float _height; 

	private float _horizontal;
	private float _vertical;

	private int _currentCol;
	private int _currentRow;
	private int _currentItem; 
	private int _previousItem;
	private float _previousTime;

	private bool _isSelectingItem;
	private InventoryItemUI _selectedInventoryItemUI; 

	private Canvas _canvas;

	void Awake() {
		_items = new ArrayList();
		_canvas = gameObject.transform.parent.GetComponent<Canvas>();
		_buildUI();

		EventCenter.Instance.OnInventoryAdded += OnInventoryAdded;
	}

	void Update() {
		if(_canvas.enabled) {
			_checkInput();
        }
    }

    public void OnInventoryAdded(string itemName) {
    	_setItemUI(itemName);	
    }

	public void OnInventoryRemoved(string itemName) {
		_resetItemUI(itemName);
	}
	
    private void _setItemUI(string itemName) {
    	if(_occupiedItems == (_numColumns * _numRows)) {
    		return;
    	}
    	var item = Inventory.Instance.GetItem(itemName);
    	var itemUI = _items[_occupiedItems] as GameObject;

    	if(itemUI != null) {
	    	var ui = itemUI.GetComponent<InventoryItemUI>();

	    	// itemUI.name = item.name;
			itemUI.name = itemName;
	    	// Debug.Log("_occupiedItems = " + _occupiedItems + ", ui = " + ui + ", _items.Count = " + _items.Count);
	    	if(ui != null) {
	    		ui.SetName(item.GetName());
	    		ui.SetCount(1);
		    	if(item.Thumbnail != null) {
			    	ui.SetThumbnail(item.Thumbnail);
		    	}
		    	if(_occupiedItems == 0) {
		    		ui.SetFocus(true);
		    	}
		    	_occupiedItems++;
		    }
    	}
    }

	private void _resetItemsUI(string itemName) {
		ArrayList tempItems = new ArrayList();
		var itemUI;
		var ui;
		var item;
		var inventory = Inventory.Instance;
		
		for(int i = 0; i < _occupiedItems; i++) {
			itemUI = _items[i] as GameObject;
			ui = itemUI.GetComponent<InventoryItemUI>();
			
			if(ui.name != itemName) {
				tempItems.Add(ui.name);
			}
			ui.Reset();
		}
		
		for(i = 0; i < tempItems; i++) {
			_setItemUI(tempItems[i]);
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

			item.name = itemName;

			rect.localPosition = new Vector3(x, y, 0);

			_items.Add(item);

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
					var itemUI = _items[_currentItem] as GameObject;
					if(itemUI != null) {
						_isSelectingItem = true;
						_selectedInventoryItemUI = itemUI.GetComponent<InventoryItemUI>();
						_selectedInventoryItemUI.Select();
					}
				} else {
					_selectedInventoryItemUI.SelectControlButton();
				}
			} else if(_isSelectingItem) {
				if(CrossPlatformInputManager.GetButtonDown("Cancel")) {
					_selectedInventoryItemUI.Deselect();
					_isSelectingItem = false;
				} else if(_checkDelayedAxisInput("vertical")) {
					if(_vertical > 0) {
						_selectedInventoryItemUI.SetControlButtonFocus(false);
					} else {
						_selectedInventoryItemUI.SetControlButtonFocus(true);
					}
				}
			} else if(!_isSelectingItem) {
				if(_checkDelayedAxisInput()) {
					if(_horizontal != 0) {
						_calculateCol(_horizontal);
					} else if(_vertical != 0) {
						_calculateRow(_vertical);
					}
					_currentItem = (_currentRow * _numColumns) + _currentCol;
					var item = _items[_currentItem] as GameObject;
					
					if(item != null) {
						item.GetComponent<InventoryItemUI>().SetFocus(true);
					}
					var prevItem = _items[_previousItem] as GameObject;
					if(prevItem != null) {
						prevItem.GetComponent<InventoryItemUI>().SetFocus(false);
					}
					_previousItem = _currentItem;
				}
			}
	
    	}
    }

	private bool _checkDelayedAxisInput(string axis = "both") {
		bool changed = false;
		_horizontal = CrossPlatformInputManager.GetAxisRaw("Horizontal");
		_vertical = CrossPlatformInputManager.GetAxisRaw("Vertical");

		if(axis == "both" && (_horizontal != 0 || _vertical != 0)) {
			changed = _checkPreviousTime();
		} else if(axis == "horizontal" && _horizontal != 0) {
			changed = _checkPreviousTime();
		} else if(axis == "vertical" && _vertical != 0) {
			changed = _checkPreviousTime();
		}

		return changed;
	}

	private bool _checkPreviousTime() {
		var changed = false;
		float now = Time.realtimeSinceStartup;
		if(-(_previousTime - now) > INPUT_DELAY) {
			changed = true;
		}
		_previousTime = now;
		return changed;
	}

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
		if(_currentCol < (_numColumns - 1)) {
			_currentCol++;
		} else {
			_currentCol = 0;
			if(isCalcCalled) {
				_incrementRow(false);
			}
		}
    }

    private void _decrementCol(bool isCalcCalled) {
		if(_currentCol > 0) {
			_currentCol--;
		} else {
			_currentCol = (_numColumns - 1);
			if(isCalcCalled) {
				_decrementRow(false);
			}
		}
    }

    private void _incrementRow(bool isCalcCalled) {
        if(_currentRow < (_numRows - 1)) {
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
            _currentRow = (_numRows - 1);
            if(isCalcCalled) {
	            _decrementCol(false);
            }
        }
    }
}
