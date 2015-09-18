using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InteractiveItemUI : MonoBehaviour {

	[SerializeField] private Text _message;

	void Awake() {
		EventCenter.Instance.OnNearInteractiveItem += this.OnNearInteractiveItem;
	}
	
	void OnNearInteractiveItem(InteractiveItem item, bool isFocused) {
		if (isFocused && item.ItemName != null) {
			_message.text = item.ItemName;
		} else {
			_message.text = "";
		}
	}
}
