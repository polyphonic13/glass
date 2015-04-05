using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {

	public static GameControl instance;

	public float health; 

	// Use this for initialization
	void Awake () {
		if(instance == null) {
			DontDestroyOnLoad(gameObject);
			instance = this;
		} else if(this != instance) {
			Destroy(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
