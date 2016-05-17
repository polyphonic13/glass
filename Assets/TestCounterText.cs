using UnityEngine;
using System.Collections;
using Polyworks; 
using UnityEngine.UI;

public class TestCounterText : MonoBehaviour {

	private const string BASE_TEXT = "Game Data Counter: "; 
	private Text _text;

	// Use this for initialization
	void Start () {
		_text = GetComponent<Text> ();
		_text.text = BASE_TEXT + (GameController.Instance.gameData.count.ToString());	
	}
	
	// Update is called once per frame
	void Update () {
		_text.text = BASE_TEXT + (GameController.Instance.gameData.count.ToString());	
	}
}
