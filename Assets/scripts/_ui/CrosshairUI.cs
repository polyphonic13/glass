using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Polyworks;

public class CrosshairUI : MonoBehaviour {

	public Image image;
	public Sprite[] icons;
	public int defaultIcon;
	
	public void OnNearItem(Item item, bool isFocused) {
		if (isFocused && item.icon != null) {
			image.sprite = icons[item.icon];
		} else {
			image.sprite = icons[defaultIcon];
		}
	}
	void Awake () {
		EventCenter.Instance.OnNearItem += this.OnNearItem;
	}

}
