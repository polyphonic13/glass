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
		setActive(false);
	}

	public void setActive(bool active) {
		if(active) {
			_itemBg.color = _active;
		} else {
			_itemBg.color = _inactive;
		}
	} 

	public void setCount(int count) {
		itemCount.text = "x" + count;
	}

	public void setThumbnail(Sprite thumbnail) {
		itemThumbnail.sprite = thumbnail;
	}
}
