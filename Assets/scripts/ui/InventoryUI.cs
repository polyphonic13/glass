using UnityEngine;
using System.Collections;
using UnitySampleAssets.CrossPlatformInput;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {

	[SerializeField] private GameObject _inventoryItem; 

	[SerializeField] private int _numColumns = 5;
	[SerializeField] private int _numRows = 3;

	float debounce = 0.0f;
	float repeat = 0.1f;  
	// reduce to speed up auto-repeat input
    
	//	private Hashtable _uiItems;
	private ArrayList _items;

	private float _width; 
	private float _height; 

	private float _startX = -410f;
	private float _startY = 115f;

	private int _currentCol = 0;
	private int _currentRow = 0;
	private int _currentItem = 0; 
	private int _previousItem = 0;

	private Canvas _canvas;

	public void Awake() {
		RectTransform containerRectTransform = this.GetComponent<RectTransform>();
		_width = containerRectTransform.rect.width / _numColumns;
		_height = containerRectTransform.rect.height / _numRows;
		_canvas = this.gameObject.transform.parent.GetComponent<Canvas>();

		Debug.Log("InventoryUI/Awake, containerRectTransform = " + containerRectTransform.rect);
//		_uiItems = new Hashtable();
		_items = new ArrayList();

		int row = 0;
		int col = 0;
		int total = _numColumns * _numRows;

		for(int i = 0; i < total; i++) {
			float x = _startX + (_width * col);
			float y = _startY + -(_height * row);
//			Debug.Log("item["+col+","+row+"] x = " + x + ", y = " + y);
			string name = "item" + i;
			GameObject item = Instantiate(_inventoryItem) as GameObject;
	
			item.transform.SetParent(this.gameObject.transform, false);
			item.name = name;

			InventoryItemUI itemUI = item.GetComponent<InventoryItemUI>();
			RectTransform rect = item.GetComponent<RectTransform>();

			itemUI.setCount(i);
			if(i == 0) {
				itemUI.setActive(true);
			}

			rect.localPosition = new Vector3(x, y, 0);

			_items.Add(item);
//			_uiItems.Add(name, item);
			col++;
			if(col % _numColumns == 0) {
				row++;
				col = 0;
			}
		}
	}

	void Update() {
		if(_canvas.enabled) {
			float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
			float vertical = CrossPlatformInputManager.GetAxis("Vertical");
            float now = Time.realtimeSinceStartup;
			bool changed = false;

			if (Mathf.Abs(horizontal) < 0.1f || Mathf.Abs(horizontal) < 0.1f) {
				debounce = 0.0f;
			}

			if(horizontal != 0) {
				changed = true;
				int col;
				if(horizontal < 0) {
					if(_currentCol > 0) {
						_currentCol--;
					} else {
						_currentCol = (_numColumns - 1);
					}
				} else {
					if(_currentCol < (_numColumns - 1)) {
						_currentCol++;
					} else {
						_currentCol = 0;
					}
                }
            }
            
            if(vertical != 0) {
				changed = true;
				int row; 
				if(vertical < 0) {
					if(_currentRow > 0) {
						_currentRow--;
					} else {
						_currentRow = (_numRows - 1);
					}
				} else {
					if(_currentRow < (_numRows - 1)) {
						_currentRow++;
					} else {
						_currentRow = 0;
					}
                }
            }
            

//            if (now - debounce > repeat) {
//					if(horizontal < 0) {
//						if(_currentItem > 0) {
//							_currentItem--;
//						} else {
//							_currentItem = (_items.Count - 1);
//						}
//					} else {
//						if(_currentItem < (_items.Count - 1)) {
//							_currentItem++;
//						} else {
//							_currentItem = 0;
//						}
//					}
			if(changed) {
				_currentItem = (_currentRow * _numColumns) + _currentCol;
				Debug.Log("cur = " + _currentItem + ", total = " + _items.Count);
				GameObject item = _items[_currentItem] as GameObject;
				
				if(item != null) {
					item.GetComponent<InventoryItemUI>().setActive(true);
				}
				GameObject prevItem = _items[_previousItem] as GameObject;
				if(prevItem != null) {
					prevItem.GetComponent<InventoryItemUI>().setActive(false);
				}
				_previousItem = _currentItem;
            }
            //                }
            //            }
        }
    }
}
