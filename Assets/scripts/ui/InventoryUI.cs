using UnityEngine;
using System.Collections;
using UnitySampleAssets.CrossPlatformInput;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {

	[SerializeField] private GameObject _inventoryItem; 

	[SerializeField] private int _numColumns = 5;
	[SerializeField] private int _numRows = 3;

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

			if(horizontal != 0) {
				if(horizontal < 0) {
					if(_currentItem > 0) {
						_currentItem--;
					} else {
						_currentItem = _items.Count;
					}
				} else {
					if(_currentItem < _items.Count) {
						_currentItem++;
					} else {
						_currentItem = 0;
					}
				}

				GameObject item = _items[_currentItem] as GameObject;
				Debug.Log("item = " + item);
				Debug.Log("_items = " + _items);
                
                if(item != null) {
					item.GetComponent<InventoryItemUI>().setActive(true);
				}
				GameObject prevItem = _items[_previousItem] as GameObject;
				if(prevItem != null) {
					prevItem.GetComponent<InventoryItemUI>().setActive(false);
                }
				_previousItem = _currentItem;
			}
		}
    }
}
