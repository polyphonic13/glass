using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InteractiveItemUI : MonoBehaviour {

	[SerializeField] private Text _message;
	private CanvasGroup _group;

	void Awake() {
		_group = GetComponent<CanvasGroup> ();
		EventCenter.Instance.OnNearInteractiveItem += this.OnNearInteractiveItem;
	}
	
	void OnNearInteractiveItem(InteractiveItem item, bool isFocused) {
		if (isFocused && item.ItemName != null) {
//			Debug.Log ("InteractiveItemUI/OnNearInteractiveItem, item = " + item.ItemName + ", isFocused = " + isFocused + ", _message.text = " + _message.text);
			_group.alpha = 1;
			_message.text = item.ItemName;
		} else {
			_group.alpha = 0;
			_message.text = "";
		}
	}
}
