using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Polyworks;

public class InteractiveItemUI : MonoBehaviour {

	[SerializeField] private Text _message;
	private CanvasGroup _group;

	void Awake() {
		_group = GetComponent<CanvasGroup> ();
		EventCenter.Instance.OnNearItem += this.OnNearItem;
	}
	
	void OnNearItem(Item item, bool isFocused) {
		if (isFocused && item.displayName != null) {
//			// Debug.Log ("InteractiveItemUI/OnNearItem, item = " + item.name + ", isFocused = " + isFocused + ", _message.text = " + _message.text);
			_group.alpha = 1;
			_message.text = item.displayName;
		} else {
			_group.alpha = 0;
			_message.text = "";
		}
	}
}
