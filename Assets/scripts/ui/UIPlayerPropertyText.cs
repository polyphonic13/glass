using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIPlayerPropertyText : MonoBehaviour {

	public string property;
	public string prefix = "";

	private Text _text;

	void Awake () {
		_text = GetComponent<Text>();
		EventCenter.instance.onPlayerPropertyUpdated += this.onPlayerPropertyUpdated;
		this.updateText();
	}
	
	void onPlayerPropertyUpdated(string prop, float val) {
		if(prop == property) {
//			Debug.Log("UIPlayerPropertyText[" + prefix + "]/onPlayerPropertyUpdated, prop = " + prop + ", val = " + val);
			this.updateText();
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
