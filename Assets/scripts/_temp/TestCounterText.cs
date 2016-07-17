using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Polyworks; 

public class TestCounterText : MonoBehaviour {

	private const string BASE_TEXT = "Game Data Counter: "; 
	private Text _text;

	// Use this for initialization
	void Start () {
		_text = GetComponent<Text> ();
		_text.text = BASE_TEXT + (Game.Instance.gameData.count.ToString());	
	}
	
	// Update is called once per frame
	void Update () {
		_text.text = BASE_TEXT + (Game.Instance.gameData.count.ToString());	
	}
}
