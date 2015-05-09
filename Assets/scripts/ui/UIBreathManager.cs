using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIBreathManager : MonoBehaviour {
	
	private Text _text;
	
	void Awake () {
		_text = GetComponent<Text>();

//		GameControl.instance.onBreathUpdated += this.onBreathUpdated;
	}
	
	void Update () {
		_text.text = "Breath: " + Mathf.Round(GameControl.instance.remainingBreath);
	}
	
	void onBreathUpdated(float val) {
		this.updateBreath();
	}
	
	void updateBreath() {
		Debug.Log("UIHealthManager/updateBreath, breath = " + GameControl.instance.remainingBreath);
		var breath = Mathf.Round(GameControl.instance.remainingBreath);
		if(breath < 0) {
			breath = 0;
		}
		_text.text = "Health: " + breath;
	}
}
