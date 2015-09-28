using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CrosshairUI : MonoBehaviour {

	[SerializeField] private Image _icon;
	[SerializeField] private Sprite _defaultSprite;

	void Awake () {
		EventCenter.Instance.OnNearInteractiveItem += this.OnNearInteractiveItem;
	}

	void OnNearInteractiveItem(InteractiveItem item, bool isFocused) {
		if (isFocused && item.Icon != null) {
			_icon.sprite = item.Icon;
		} else {
			_icon.sprite = _defaultSprite;
		}
	}
}
