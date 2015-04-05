using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {

	public static GameControl instance;

	public delegate void HealthUpdateHandler(float val);
	public event HealthUpdateHandler onHealthUpdated;
	
	public float health; 
	public int targetRoom;

	private Vector3 _cave01Start = new Vector3(133f, 4f, 1.4f);
	private Vector3 _houseFloor2BedroomNorth = new Vector3(0f, 0f, 0f);

	private Vector3[] _startingPositions = new [] {
		new Vector3(133f, 4f, 1.4f),
		new Vector3(-4f, 15.5f, -30f)
	};

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

	public Vector3 getStartingPosition() {
		return _startingPositions[targetRoom];
	}
}
