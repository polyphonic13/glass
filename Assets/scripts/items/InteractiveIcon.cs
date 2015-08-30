using UnityEngine;
using UnityEngine.UI;

public class InteractiveIcon : MonoBehaviour {

	public GameObject interactiveIcon; 

	private bool _isJustChanged = false;
	private InteractiveItem _interactiveItem; 
	private Text _itemNameText; 

	void Awake() {
		_interactiveItem = gameObject.GetComponent<InteractiveItem>();
		var itemNameObj = transform.Find("item_icon_ui/item_name");
		_itemNameText = itemNameObj.GetComponent<Text>();

		if(interactiveIcon != null) {
			interactiveIcon.SetActive(false);
			_itemNameText.text = _interactiveItem.GetName();
		} else {
			_itemNameText.text = "";
		}
	}
	
	void Update() {
		if(_interactiveItem.IsEnabled) {
			interactiveIcon.transform.rotation = Quaternion.LookRotation(interactiveIcon.transform.position - _interactiveItem.GetCamera().transform.position);
			if(_interactiveItem.CheckProximity()) {
				_turnOnIcon();
			} else if(_isJustChanged){
				_turnOffIcon();
			}
		}
	}
	
	void _turnOnIcon() {
		interactiveIcon.SetActive(true);
		_isJustChanged = true;
	}
	
	void _turnOffIcon() {
		interactiveIcon.SetActive(false);
		_isJustChanged = false;
	}
}
