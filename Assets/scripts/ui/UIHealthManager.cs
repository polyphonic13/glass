using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIHealthManager : MonoBehaviour {

	private Text _text;

	void Awake () {
		_text = GetComponent<Text>();
//		GameControl.instance.onHealthUpdated += this.onHealthUpdated;
	}
	
	void Update () {
		_text.text = "Health: " + Mathf.Round(GameControl.instance.health);
	}

	void onHealthUpdated(float val) {
		this.updateHealth();
	}

	void updateHealth() {
		Debug.Log("UIHealthManager/updateHealth, health = " + GameControl.instance.health);
		var health = Mathf.Round(GameControl.instance.health);
		if(health < 0) {
			health = 0;
		}
		_text.text = "Health: " + health;
	}
}
