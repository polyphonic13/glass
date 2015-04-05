using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIHealthManager : MonoBehaviour {

//	public static float health;

	private Text _text;

	// Use this for initialization
	void Awake () {
		_text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		_text.text = "Health: " + Mathf.Round(GameControl.instance.health);
	}
}
