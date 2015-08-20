using UnityEngine;
using System.Collections;
using UnitySampleAssets.CrossPlatformInput;

public class InventoryUI : MonoBehaviour {

	[SerializeField] private GameObject _inventoryItem; 

	[SerializeField] private int _numColumns = 5;
	[SerializeField] private int _numRows = 3;

	private const float START_X = -410f;
	private const float START_Y = 115f;
	private const float INPUT_DELAY = .03f;

	private ArrayList _items;
	private int _occupiedItems;

	private float _width; 
	private float _height; 

	private int _currentCol;
	private int _currentRow;
	private int _currentItem; 
	private int _previousItem;
	private float _previousTime;

	private Canvas _canvas;

	void Awake() {
		_items = new ArrayList();
		_buildUI();

		EventCenter.Instance.OnInventoryAdded += OnInventoryAdded;
	}

	void Update() {
		if(_canvas.enabled) {
			_handleInput();
        }
    }

    public void OnInventoryAdded(string itemName) {
    	// Debug.Log("InventoryUI/OnInventoryAdded, itemName = " + itemName);
    	_setItemUI(itemName);	
    }

    private void _setItemUI(string itemName) {
    	if(_occupiedItems == (_numColumns * _numRows)) {
    		return;
    	}
    	var item = Inventory.Instance.GetItem(itemName);
    	var itemUI = _items[_occupiedItems] as GameObject;

    	if(itemUI != null) {
	    	var ui = itemUI.GetComponent<InventoryItemUI>();

	    	Debug.Log("_occupiedItems = " + _occupiedItems + ", ui = " + ui + ", _items.Count = " + _items.Count);
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
		_canvas = gameObject.transform.parent.GetComponent<Canvas>();

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

    private void _handleInput() {
		float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
		float vertical = CrossPlatformInputManager.GetAxis("Vertical");
        
		if(horizontal != 0 || vertical != 0) {
			float now = Time.realtimeSinceStartup;

			if(-(_previousTime - now) > INPUT_DELAY) {
				var changed = false;
				if(horizontal != 0) {
					changed = true;
					_calculateCol(horizontal);
				}
				
				if(vertical != 0) {
					changed = true;
					_calculateRow(vertical);
                }

                if(changed) {
                    _currentItem = (_currentRow * _numColumns) + _currentCol;
                    var item = _items[_currentItem] as GameObject;
                    
                    if(item != null) {
						item.GetComponent<InventoryItemUI>().SetFocus(true);
					}
					var prevItem = _items[_previousItem] as  GameObject;
                    if(prevItem != null) {
                        prevItem.GetComponent<InventoryItemUI>().SetFocus(false);
                    }
                    _previousItem = _currentItem;
				}
			}
			_previousTime = now;
		}
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
