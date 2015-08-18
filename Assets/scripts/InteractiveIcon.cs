using UnityEngine;
using UnityEngine.UI;

public class InteractiveIcon : MonoBehaviour {

	public GameObject interactiveIcon; 

	private bool _isJustChanged = false;
	private InteractiveElement _interactiveElement; 
	private Text _itemNameText; 

	void Awake() {
		_interactiveElement = gameObject.GetComponent<InteractiveElement>();
		var itemNameObj = transform.Find("item_icon_ui/item_name");
		_itemNameText = itemNameObj.GetComponent<Text>();

		if(interactiveIcon != null) {
			interactiveIcon.SetActive(false);
			_itemNameText.text = _interactiveElement.getName();
		} else {
			_itemNameText.text = "";
		}
	}
	
	void Update() {
		if(_interactiveElement.IsEnabled) {
			interactiveIcon.transform.rotation = Quaternion.LookRotation(interactiveIcon.transform.position - _interactiveElement.getCamera().transform.position);
			if(_interactiveElement.CheckProximity()) {
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
