using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameControl : MonoBehaviour {

	public float health = 100f; 
	public float breath = 120f;
	public float stamina = 5f;

	public string currentTargetScene = "";

	public bool hasFlashlight = false; 

	public int targetRoom = -1;

	public Hashtable inventoryItems; 

	public float RemainingHealth { get; set; }
	public float RemainingBreath { get; set; }
	public float RemainingStamina { get; set; } 

	private const string LOADING_SCENE = "loading";
	private int _loadingPause = 2; 

	private string[] _playerScenes = {
		"house_floor02a",
		"cave01",
		"climb_test"
	};

	public static GameControl Instance;

	private Vector3[] _startingPositions = new [] {
		new Vector3(133f, 4f, 1.4f),
		new Vector3(-4f, 15.5f, -30f)
	};

	void Awake() {
		if(Instance == null) {
			DontDestroyOnLoad(gameObject);
			Instance = this;
		} else if(this != Instance) {
			Destroy(gameObject);
		}

		Cursor.visible = false;

		Scene currentScene = SceneManager.GetActiveScene ();

		if (currentScene.name == LOADING_SCENE && Instance.currentTargetScene != "") {
			Instance.StartCoroutine(_pauseDuringLoading());
		} else {
			for (int i = 0; i < _playerScenes.Length; i++) {
				if(_playerScenes[i] == currentScene.name) {
					_initPlayer();
					break;
				}
			}
		}			
	}

	private IEnumerator _pauseDuringLoading() {
		yield return new WaitForSeconds (_loadingPause);
		string toLoad = Instance.currentTargetScene;
		Instance.currentTargetScene = "";
		_loadScene (toLoad);
	}

	private void _initPlayer() {
		RemainingHealth = health;
		RemainingBreath = breath;
		RemainingStamina = stamina;
		
		var ec = EventCenter.Instance;
		ec.UpdatePlayerProperty ("health", RemainingHealth);
		ec.UpdatePlayerProperty ("breath", RemainingBreath);
		ec.UpdatePlayerProperty ("stamina", RemainingStamina);

		Inventory.Instance.InitPlayer ();
	}

	private void _loadScene(string scene) {
//		Debug.Log ("going to load: " + scene);
		SceneManager.LoadScene (scene);
	}

	public float GetProperty(string prop) {
		float val = 0;

		switch(prop) {
			case "health":
				val = health;
				break;

			case "breath":
				val = RemainingBreath;
				break;

			case "stamina":
				val = RemainingStamina;
				break;

		}
		return val;
	}


	public void ChangeScene(string scene, int room = -1) {
		Instance.inventoryItems = Inventory.Instance.GetAll ();
		Instance.currentTargetScene = scene;
//		Debug.Log ("GameControl/ChangeScene, currentTargetScene = " + Instance.currentTargetScene);
		targetRoom = room;
		_loadScene (LOADING_SCENE);
	}
		
	public void UpdateHealth(float val) {
//		Debug.Log ("GameControl/UpdateHealth, val = " + +val);
		health = val; 
		_postHealthUpdate();
	}

	public void DamagePlayer(float val) {
		health -= val;
		_postHealthUpdate();
	}

	public void UpdateBreath(float val) {
		RemainingBreath = val;
		EventCenter.Instance.UpdatePlayerProperty("breath", RemainingBreath);
    }

	public void UpdateHeldBreathTime(float val) {
        RemainingBreath = breath - val;
        EventCenter.Instance.UpdatePlayerProperty("breath", RemainingBreath);
    }

	public void ResetBreath() {
		RemainingBreath = breath;
		EventCenter.Instance.UpdatePlayerProperty("breath", RemainingBreath);
	}

	public void UpdateStamina(float val) {
		RemainingStamina = val;
		EventCenter.Instance.UpdatePlayerProperty("stamina", RemainingStamina);
    }

	public void ResetStamina() {
		RemainingStamina = stamina;
	}

	private void _postHealthUpdate() {
		EventCenter.Instance.UpdatePlayerProperty("health", health);
		
		if(health < 1) {
			_die();
		}
	}
	
	private void _die() {
		Application.LoadLevel("game_over");

	}
}
