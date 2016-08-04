using UnityEngine;
using UnityEngine.UI;

public class UIPlayer_propertyText : MonoBehaviour {

	public string _property;
	public string _prefix = "";

	private Text _text;

	void Awake() {
		_text = GetComponent<Text>();
//		EventCenter.Instance.OnPlayerPropertyUpdated += OnPlayerPropertyUpdated;
		updateText();
	}
	
	void OnPlayerPropertyUpdated(string prop, float val) {
		if(prop == _property) {
//			// Debug.Log("UIPlayer_propertyText[" + _prefix + "]/OnPlayerPropertyUpdated, prop = " + prop + ", val = " + val);
			updateText();
		}
	}

	void updateText() {
//		var value = Mathf.Round(Game.Instance.GetProperty(_property));
//		if(value < 0) {
//			value = 0;
//		}
//		_text.text = _prefix + value;
	}
}
