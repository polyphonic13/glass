using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {

	public static GameControl instance;

	public delegate void HealthUpdateHandler(float val);
	public event HealthUpdateHandler onHealthUpdated;

	public delegate void BreathUpdateHandler(float val);
	public event BreathUpdateHandler onBreathUpdated;

	public float health = 100f; 
	public float breath = 120f;
	public float remainingBreath;

	public int targetRoom;

	private float _currentBreath;

//	private Vector3 _cave01Start = new Vector3(133f, 4f, 1.4f);
//	private Vector3 _houseFloor2BedroomNorth = new Vector3(0f, 0f, 0f);

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

		remainingBreath = breath;
	}

	public void updateHealth(float val) {
		health = val; 

		if(onHealthUpdated != null) {
			onHealthUpdated(val);
		}

		if(health < 0) {
			_die();
		}
	}

	public void damagePlayer(float val) {
		health -= val;
		if(onHealthUpdated != null) {
			onHealthUpdated(health);
		}

		if(health < 0) {
			_die();
		}
	}

	public void updateBreath(float val) {
		remainingBreath = val;
		if(onBreathUpdated != null) {
			onBreathUpdated(remainingBreath);
		}
	}

	public void updateHeldBreathTime(float val) {

		remainingBreath = breath - val;

		Debug.Log("GameControl/updateHeldBreathTime, val = " + val + ", remainingBreath = " + remainingBreath);
		if(onBreathUpdated != null) {
			onBreathUpdated(remainingBreath);
		}
	}

	public void resetBreath() {
		remainingBreath = breath;
	}

	public Vector3 getStartingPosition() {
		Debug.Log("GameControl/getStartingPosition, val = " + _startingPositions[targetRoom]);
		return _startingPositions[targetRoom];
	}

	private void _die() {
		Application.LoadLevel("game_over");

	}
}
