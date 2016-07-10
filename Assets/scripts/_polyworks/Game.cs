using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Reflection;
using System;

namespace Polyworks {
	public class Game : MonoBehaviour {

		public GameData gameData;

		public string dataFilename = "game_data.dat"; 

		public string currentTargetScene = "";

		public bool isCursorless = true;

		private Player _player;
		private Inventory _playerInventory; 

		private DataIOController _dataIOController; 

		public static Game Instance;

		public virtual void Init() {

			Scene currentScene = SceneManager.GetActiveScene ();
			string currentSceneName = currentScene.name;

			_dataIOController = new DataIOController ();

			if (isCursorless) {
				Cursor.visible = false;
			}
			if (Instance.gameData.tasks == null) {
				Instance.gameData.tasks = new Hashtable ();
			}

			if (Instance.gameData.items == null) {
				// Debug.Log ("have to make a new hashtable for Instance.gameData.items");
				Instance.gameData.items = new Hashtable ();
			}

			if (Instance.gameData.clearedScenes == null) {
				// Debug.Log ("CLEARED SCENES IS NULL");
				Instance.gameData.clearedScenes = new Hashtable ();
			}

			Instance.gameData.currentScene = currentSceneName; 
			Hashtable items = Instance.gameData.items;
			_playerInventory.Init (items);

			if (_getIsLevel(currentSceneName)) {
				_initLevel (currentSceneName);
			} else {
				EventCenter.Instance.SceneInitializationComplete (currentSceneName);
			}
			EventCenter.Instance.OnChangeScene += OnChangeScene;
		}

		public virtual Inventory GetPlayerInventory() {
			if (_playerInventory == null) {
				_playerInventory = GetComponent<Inventory> ();
			}
			return _playerInventory;
		}

		public void Save() {
			Scene currentScene = SceneManager.GetActiveScene ();
			Instance.gameData.items = _playerInventory.GetAll ();
			_dataIOController.Save (Application.persistentDataPath + "/" + dataFilename, Instance.gameData);
		}

		public void Load() {
			GameData data = _dataIOController.Load (Application.persistentDataPath + "/" + dataFilename);
			if (data != null) {
				Instance.gameData = data;

				_playerInventory.Init (data.items);
				if (data.currentScene != "") {
					ChangeScene (data.currentScene);
				} else {
					Scene currentScene = SceneManager.GetActiveScene ();
					string currentSceneName = currentScene.name;
					ChangeScene (currentSceneName);
				}
			}
		}

		public void ChangeScene(string scene) {
			// Debug.Log ("Game/ChangeScene, scene = " + scene + ", gameData.items.Count = " + Instance.gameData.items.Count);
			Scene currentScene = SceneManager.GetActiveScene ();

			if (scene != currentScene.name) {
				Instance.gameData.items = _playerInventory.GetAll ();
				Instance.currentTargetScene = scene;
				LevelController levelController = GameObject.Find("level_controller").GetComponent<LevelController>();
				levelController.Cleanup();
				_loadScene (scene);
			} else if (_getIsLevel (currentScene.name)) {
				_initLevel (currentScene.name);
			}
		}

		public void OnChangeScene(string scene) {
			ChangeScene (scene);
		}

		public void Increment() {
			Instance.gameData.count++;

			Scene currentScene = SceneManager.GetActiveScene ();
			if (currentScene.name == "game_control_test02") {
				EventCenter.Instance.UpdateIntTask ("incrementCount", Instance.gameData.count);
			}
		}

		private void Awake() {
			if(Instance == null) {
				DontDestroyOnLoad(gameObject);
				Instance = this;
			} else if(this != Instance) {
				Destroy(gameObject);
			}
			Init ();
		}

		private void _initLevel(string currentSceneName) {
			GameObject playerObj = GameObject.Find("player");
			if (_player != null) {
				_player = playerObj.GetComponent<Player> ();
				_playerInventory = playerObj.GetComponent<Inventory> ();
			}

			LevelController levelController = GameObject.Find("level_controller").GetComponent<LevelController>();
			levelController.Init (Instance.gameData);
			EventCenter.Instance.SceneInitializationComplete (currentSceneName);
		}

		private void _loadScene(string scene) {
			SceneManager.LoadScene (scene);
		}

		private bool _getIsLevel(string sceneName) {
			return LevelUtils.Has (sceneName, Instance.gameData.levels);
		}
	}
}
