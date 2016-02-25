using UnityEngine;

public class GameControl : MonoBehaviour {

	public float _health = 100f; 
	public float _breath = 120f;
	public float _stamina = 5f;

	public int targetRoom = -1;

	public float RemainingHealth { get; set; }
	public float RemainingBreath { get; set; }
	public float RemainingStamina { get; set; } 

	private string[] _playerScenes = {
		"house_floor02a",
		"cave01",
		"climb_test"
	};

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
	
	private Vector3[] _startingPositions = new [] {
		new Vector3(133f, 4f, 1.4f),
		new Vector3(-4f, 15.5f, -30f)
	};

	void Awake() {
		if(_instance == null) {
			DontDestroyOnLoad(gameObject);
			_instance = this;
		} else if(this != _instance) {
			Destroy(gameObject);
		}

		Cursor.visible = false;

		for (int i = 0; i < _playerScenes.Length; i++) {
			if(_playerScenes[i] == Application.loadedLevelName) {
				_initPlayer();
				break;
			}
		}
	}

	private void _initPlayer() {
		RemainingHealth = _health;
		RemainingBreath = _breath;
		RemainingStamina = _stamina;
		
		var ec = EventCenter.Instance;
		ec.UpdatePlayerProperty ("_health", RemainingHealth);
		ec.UpdatePlayerProperty ("_breath", RemainingBreath);
		ec.UpdatePlayerProperty ("_stamina", RemainingStamina);

		Inventory.Instance.InitPlayer ();
	}

	public float GetProperty(string prop) {
		float val = 0;

		switch(prop) {
			case "_health":
				val = _health;
				break;

			case "_breath":
				val = RemainingBreath;
				break;

			case "_stamina":
				val = RemainingStamina;
				break;

		}
		return val;
	}


	public void ChangeScene(string tgt, int room = -1) {
		targetRoom = room;
		Application.LoadLevel(tgt);
	}

	public void UpdateHealth(float val) {
		Debug.Log ("GameControl/UpdateHealth, val = " + +val);
		_health = val; 
		_postHealthUpdate();
	}

	public void DamagePlayer(float val) {
		_health -= val;
		_postHealthUpdate();
	}

	public void UpdateBreath(float val) {
		RemainingBreath = val;
		EventCenter.Instance.UpdatePlayerProperty("_breath", RemainingBreath);
    }

	public void UpdateHeldBreathTime(float val) {
        RemainingBreath = _breath - val;
        EventCenter.Instance.UpdatePlayerProperty("_breath", RemainingBreath);
    }

	public void ResetBreath() {
		RemainingBreath = _breath;
		EventCenter.Instance.UpdatePlayerProperty("_breath", RemainingBreath);
	}

	public void UpdateStamina(float val) {
		RemainingStamina = val;
		EventCenter.Instance.UpdatePlayerProperty("_stamina", RemainingStamina);
    }

	public void ResetStamina() {
		RemainingStamina = _stamina;
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
