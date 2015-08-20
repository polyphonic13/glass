using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour {

    [SerializeField] private Text itemCount;
	[SerializeField] private Image itemThumbnail;

	private Image _itemBg;
    private Color32 _active;
	private Color32 _inactive; 

	void Awake() {
		_active = new Color32(188, 188, 188, 100);
		_inactive = new Color32(0, 0, 0, 100);
		_itemBg = GetComponent<Image>();
		SetFocus(false);
	}

	public void SetFocus(bool active) {
		if(active) {
			_itemBg.color = _active;
		} else {
			_itemBg.color = _inactive;
		}
	} 

	public void SetCount(int count) {
		itemCount.text = "x" + count;
	}

	public void SetThumbnail(Sprite thumbnail) {
		Debug.Log("InventoryItemUI/SetThumbnail, thumbnail = " + thumbnail + ", sprite = " + itemThumbnail.sprite);
		itemThumbnail.sprite = thumbnail;
		itemThumbnail.color = Color.white;
		Debug.Log("InventoryItemUI/SetThumbnail, thumbnail = " + thumbnail + ", sprite = " + itemThumbnail.sprite);
	}

	public void ClearThumbnail() {
		itemThumbnail.sprite = null;
		itemThumbnail.color = Color.black;
	}
}
