using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {

	public static GameControl instance;

	public delegate void HealthUpdateHandler(float val);
	public event HealthUpdateHandler onHealthUpdated;


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
	
	public void updateHealth(float val) {
		health = val; 

		if(onHealthUpdated != null) {
			onHealthUpdated(val);
		}
	}
}
