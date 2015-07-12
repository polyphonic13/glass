using UnityEngine;

public class GameControl : MonoBehaviour {

//	public static GameControl instance;

	public float _health = 100f; 
	public float _breath = 120f;
	public float _stamina = 5f;
	public float _remainingBreath;
	public float _remainingStamina; 

	public int _targetRoom;

	private static GameControl _instance;
	private GameControl() {}
	
	public static GameControl Instance {
		get {
			if(_instance == null) {
				_instance = GameObject.FindObjectOfType(typeof(GameControl)) as GameControl;      
			}
			return _instance;
		}
	}
	
	//	private Vector3 _cave01Start = new Vector3(133f, 4f, 1.4f);
//	private Vector3 _houseFloor2BedroomNorth = new Vector3(0f, 0f, 0f);

	private Vector3[] _startingPositions = new [] {
		new Vector3(133f, 4f, 1.4f),
		new Vector3(-4f, 15.5f, -30f)
	};

	// Use this for initialization
	void Awake () {
		if(_instance == null) {
			DontDestroyOnLoad(gameObject);
			_instance = this;
		} else if(this != _instance) {
			Destroy(gameObject);
		}

		_remainingBreath = _breath;
		_remainingStamina = _stamina;

		var ec = EventCenter.Instance;
		ec.UpdatePlayerProperty("_health", _health);
		ec.UpdatePlayerProperty("_breath", _remainingBreath);
		ec.UpdatePlayerProperty("_stamina", _remainingStamina);
	}

	public float GetProperty(string prop) {
		float val = 0;

		switch(prop) {
			case "_health":
				val = _health;
				break;
			case "_breath":
				val = _remainingBreath;
				break;
			case "_stamina":
				val = _remainingStamina;
				break;
		}
		return val;
	}


	public void ChangeScene(string tgt, int room) {
		_targetRoom = room;
		Application.LoadLevel(tgt);
//		var player = GameObject.Find("Player");
//		player.transform.position = GameControl.Instance.GetStartingPosition();
	}

	public void UpdateHealth(float val) {
		_health = val; 
		_postHealthUpdate();
	}

	public void DamagePlayer(float val) {
		_health -= val;
		_postHealthUpdate();
	}

	public void UpdateBreath(float val) {
		_remainingBreath = val;
		EventCenter.Instance.UpdatePlayerProperty("_breath", _remainingBreath);
    }

	public void UpdateHeldBreathTime(float val) {
        _remainingBreath = _breath - val;
        EventCenter.Instance.UpdatePlayerProperty("_breath", _remainingBreath);
    }

	public void ResetBreath() {
		_remainingBreath = _breath;
		EventCenter.Instance.UpdatePlayerProperty("_breath", _remainingBreath);
	}

	public void UpdateStamina(float val) {
		_remainingStamina = val;
		EventCenter.Instance.UpdatePlayerProperty("_stamina", _remainingStamina);
    }

	public void ResetStamina() {
		_remainingStamina = _stamina;
	}

	public Vector3 GetStartingPosition() {
//		Debug.Log("GameControl/GetStartingPosition, val = " + _startingPositions[_targetRoom]);
		return _startingPositions[_targetRoom];
	}

	private void _postHealthUpdate() {
		EventCenter.Instance.UpdatePlayerProperty("_health", _health);
		
		if(_health < 1) {
			_die();
		}
	}
	
	private void _die() {
		Application.LoadLevel("game_over");

	}
}
