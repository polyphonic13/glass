using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPlayerPropertyText : MonoBehaviour {

	public string property;
	public string prefix = "";

	private Text _text;

	void Awake () {
		_text = GetComponent<Text>();
		EventCenter.Instance.OnPlayerPropertyUpdated += OnPlayerPropertyUpdated;
		updateText();
	}
	
	void OnPlayerPropertyUpdated(string prop, float val) {
		if(prop == property) {
//			Debug.Log("UIPlayerPropertyText[" + prefix + "]/OnPlayerPropertyUpdated, prop = " + prop + ", val = " + val);
			updateText();
		}
	}

	void updateText() {
		var value = Mathf.Round(GameControl.instance.getProperty(property));
		if(value < 0) {
			value = 0;
		}
		_text.text = prefix + value;
	}
}
