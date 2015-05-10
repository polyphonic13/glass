using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {

	public static GameControl instance;

	public float health = 100f; 
	public float breath = 120f;
	public float stamina = 10f;
	public float remainingBreath;
	public float remainingStamina; 

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
		remainingStamina = stamina;

		var ec = EventCenter.instance;
		ec.updatePlayerProperty("health", health);
		ec.updatePlayerProperty("breath", remainingBreath);
		ec.updatePlayerProperty("stamina", remainingStamina);
	}

	public float getProperty(string prop) {
		float val = 0;

		switch(prop) {
			case "health":
				val = health;
				break;
			case "breath":
				val = remainingBreath;
				break;
			case "stamina":
				val = remainingStamina;
				break;
			default:
				break;
		}
		return val;
	}

	public void updateHealth(float val) {
		health = val; 
		EventCenter.instance.updatePlayerProperty("health", health);

		if(health < 0) {
			_die();
		}
	}

	public void damagePlayer(float val) {
		health -= val;
		EventCenter.instance.updatePlayerProperty("health", health);

		if(health < 0) {
			_die();
		}
	}

	public void updateBreath(float val) {
		remainingBreath = val;
		EventCenter.instance.updatePlayerProperty("breath", remainingBreath);
    }

	public void updateHeldBreathTime(float val) {
        remainingBreath = breath - val;
        EventCenter.instance.updatePlayerProperty("breath", remainingBreath);
    }

	public void resetBreath() {
		remainingBreath = breath;
	}

	public void updateStamina(float val) {
		remainingStamina = val;
		EventCenter.instance.updatePlayerProperty("stamina", remainingStamina);
    }

	public void resetStamina() {
		remainingStamina = stamina;
	}

	public Vector3 getStartingPosition() {
		Debug.Log("GameControl/getStartingPosition, val = " + _startingPositions[targetRoom]);
		return _startingPositions[targetRoom];
	}

	private void _die() {
		Application.LoadLevel("game_over");

	}
}
