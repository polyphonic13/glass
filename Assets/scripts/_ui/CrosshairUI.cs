using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CrosshairUI : MonoBehaviour {

	public Image image;
	public Sprite[] icons;
	public int defaultIcon;
	
	void Awake () {
		EventCenter.Instance.OnNearInteractiveItem += this.OnNearInteractiveItem;
	}

	void OnNearInteractiveItem(Item item, bool isFocused) {
		if (isFocused && item.data.icon != null) {
			image.sprite = item.data.icon;
		} else {
			image.sprite = defaultIcon;
		}
	}
}
