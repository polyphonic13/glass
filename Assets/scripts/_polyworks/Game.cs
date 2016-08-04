using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Reflection;
using System;

namespace Polyworks {
	public class Game : MonoBehaviour {

		public GameData gameData;

		public string dataFilename = "game_data.dat"; 
		public string playerPrefab = "player_objects";

		public bool isCursorless = true;

		public string currentTargetScene { get; set; }

		private LevelController _levelController;
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
				Instance.gameData.items = new Hashtable ();
			}

			if (Instance.gameData.clearedScenes == null) {
				Instance.gameData.clearedScenes = new Hashtable ();
			}

			Instance.gameData.currentScene = currentSceneName; 
			Hashtable items = Instance.gameData.items;

			if (_getIsLevel(currentSceneName)) {
				_initLevel (currentSceneName, items);
			} else {
				EventCenter.Instance.SceneInitializationComplete (currentSceneName);
			}

			EventCenter ec = EventCenter.Instance;
			ec.OnChangeScene += OnChangeScene;
		}

		#region handlers
		public void OnChangeScene(string scene) {
			ChangeScene (scene);
		}

		public void OnSectionChange(int section) {

		}
		#endregion
	
		#region public methods
		public virtual Inventory GetPlayerInventory() {
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

				if (data.currentScene != "") {
					// Debug.Log ("going to change scene to " + data.currentScene);
					ChangeScene (data.currentScene);
				} else {
					Scene currentScene = SceneManager.GetActiveScene ();
					string currentSceneName = currentScene.name;
					ChangeScene (currentSceneName);
				}
			}
		}

		public void ChangeScene(string scene, int section = -1) {
			Scene currentScene = SceneManager.GetActiveScene ();
			bool isLevel = _getIsLevel (currentScene.name);

			if (scene != currentScene.name) {
				if (isLevel) {
					if (_levelController != null) {
						LevelUtils.SetLevelData (currentScene.name, Instance.gameData.levels, _levelController.GetLevelData());
					}
					Instance.gameData.items = _playerInventory.GetAll ();
					Instance.gameData.targetSection = section;
				}
				_cleanUp ();

				Instance.currentTargetScene = scene;
				_loadScene (scene);
			}
		}

		public void LevelInitialized() {
			PlayerManager pm = GameObject.Find ("level_controller").GetComponent<PlayerManager> ();
			_player = pm.GetPlayer();
			_playerInventory = pm.GetInventory ();
			Scene currentScene = SceneManager.GetActiveScene ();

			GameObject inventoryObj = GameObject.Find ("inventory_ui");
			if (inventoryObj != null) {
				InventoryUI inventoryUI = inventoryObj.GetComponent<InventoryUI> ();
				inventoryUI.InitInventory(_playerInventory);
			}

			EventCenter.Instance.SceneInitializationComplete (currentScene.name);
		}

		public void Increment() {
			Instance.gameData.count++;

			Scene currentScene = SceneManager.GetActiveScene ();
			if (currentScene.name == "game_control_test02") {
				EventCenter.Instance.UpdateIntTask ("incrementCount", Instance.gameData.count);
			}
		}
		#endregion

		#region private methods
		private void Awake() {
			if(Instance == null) {
				DontDestroyOnLoad(gameObject);
				Instance = this;
			} else if(this != Instance) {
				Destroy(gameObject);
			}
			Init ();
		}

		private void _initLevel(string currentSceneName, Hashtable items) {
			_levelController = GameObject.Find("level_controller").GetComponent<LevelController>();
			_levelController.Init (Instance.gameData);
		}

		private void _loadScene(string scene) {
			SceneManager.LoadScene (scene);
		}

		private bool _getIsLevel(string sceneName) {
			return LevelUtils.Has (sceneName, Instance.gameData.levels);
		}

		private void _cleanUp() {
			_levelController = null;
			EventCenter ec = EventCenter.Instance;
			ec.OnChangeScene -= OnChangeScene;
		}
		#endregion
	}
}
