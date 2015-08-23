using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour {

	[SerializeField] private Text _itemName;
    [SerializeField] private Text _itemCount;
	[SerializeField] private Image _itemThumbnail;

	[SerializeField] private CanvasGroup _controlPanel;

	private Image _itemBg;
    private Color32 _active;
	private Color32 _inactive; 

	private bool _isFocused;
	private bool _isControlPanelOpen; 

	void Awake() {
		_controlPanel.alpha = 0;
		_active = new Color32(100, 100, 100, 100);
		_inactive = new Color32(0, 0, 0, 100);
		_itemBg = GetComponent<Image>();
		SetFocus(false);
		SetThumbnail(null);
	}

//	void Update() {
//		if(_isFocused) {
//
//		}
//	}

	public void Select() {
		_controlPanel.alpha = 1;
		_isControlPanelOpen = true;
	}

	public void Deselect() {
		_controlPanel.alpha = 0;
		_isControlPanelOpen = false;
	}

	public void ChangeControlButtonFocus(bool increment) {
		Debug.Log("InventoryItemUI["+this.name+"]/ChangeControlButtonFocus, increment = " + increment);
	}

	public void SetFocus(bool active) {
		if(active) {
			_itemBg.color = _active;
		} else {
			_itemBg.color = _inactive;
		}
		_isFocused = active;
	} 

	public void SetName(string itemName) {
		// Debug.Log("InventoryItemUI/setName: " + itemName);
		_itemName.text = itemName;
	}

	public void SetCount(int count) {
		_itemCount.text = "x" + count;
	}

	public void SetThumbnail(Sprite thumbnail) {
		if(thumbnail != null) {
			_itemThumbnail.sprite = thumbnail;
			_itemThumbnail.color = Color.white;
		} else {
			_itemThumbnail.sprite = null;
			_itemThumbnail.color = new Color32(0, 0, 0, 0);
		}
	}
}
