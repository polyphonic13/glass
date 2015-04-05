using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIHealthManager : MonoBehaviour {

//	public static float health;

	private Text _text;

	// Use this for initialization
	void Awake () {
		_text = GetComponent<Text>();
//		GameControl.instance.onHealthUpdated += this.onHealthUpdated;

//		this.updateHealth();
	}
	
	// Update is called once per frame
	void Update () {
		_text.text = "Health: " + Mathf.Round(GameControl.instance.health);
	}

	void onHealthUpdated(float val) {
		this.updateHealth();
	}

	void updateHealth() {
		Debug.Log("UIHealthManager/updateHealth, health = " + GameControl.instance.health);
		_text.text = "Health: " + Mathf.Round(GameControl.instance.health);
	}
}
