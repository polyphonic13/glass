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

		public bool isSceneInitialized = false;

		public bool isCursorless = true;

		public Inventory playerInventory { get; set; }

		private LevelController _levelController;
		private Player _player;
		private Inventory _playerInventory; 

		private DataIOController _dataIOController; 

		public static Game Instance;

		public virtual void Init() {

			Scene currentScene = SceneManager.GetActiveScene ();
			string currentSceneName = currentScene.name;
			bool isLevel = _getIsLevel (currentSceneName);
			Instance.isSceneInitialized = false;

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

			SceneChanger.Instance.Init (currentSceneName);

			if (isLevel) {
				_initLevel (currentSceneName, items);
			} else {
				_completeSceneInitialization (isLevel, currentSceneName);
			}

			EventCenter ec = EventCenter.Instance;
			ec.OnStartSceneChange += OnStartSceneChange;
			ec.OnCompleteSceneChange += OnCompleteSceneChange;
		}

		#region handlers
		public void OnStartSceneChange(string scene, int section = -1) {
			_prepForSceneChange (scene, section);
		}

		public void OnCompleteSceneChange(string scene, int section = -1) {
			_loadScene (scene);
		}

		#endregion
	
		#region public methods
		public virtual Inventory GetPlayerInventory() {
			return Instance.playerInventory;
		}

		public void Save() {
			Scene currentScene = SceneManager.GetActiveScene ();
			Instance.gameData.items = Instance.playerInventory.GetAll ();
			_dataIOController.Save (Application.persistentDataPath + "/" + dataFilename, Instance.gameData);
		}

		public void Load() {
			GameData data = _dataIOController.Load (Application.persistentDataPath + "/" + dataFilename);
			if (data != null) {
				Instance.gameData = data;

				if (data.currentScene != "") {
					_changeScene(data.currentScene, data.targetSection);
				} else {
					Scene currentScene = SceneManager.GetActiveScene ();
					string currentSceneName = currentScene.name;
					_changeScene(currentSceneName, data.targetSection);
				}
			}
		}

		public void StartGame() {
			LevelData level = Instance.gameData.levels [0];
			_changeScene(level.name, level.currentSection);
		}

		public void LevelInitialized() {
			PlayerManager pm = GameObject.Find ("level_controller").GetComponent<PlayerManager> ();
			_player = pm.GetPlayer();
			Instance.playerInventory = pm.GetInventory ();
			Scene currentScene = SceneManager.GetActiveScene ();

			GameObject inventoryObj = GameObject.Find ("inventory_ui");
			if (inventoryObj != null) {
				InventoryUI inventoryUI = inventoryObj.GetComponent<InventoryUI> ();
				inventoryUI.InitInventory(Instance.playerInventory);
			}


			_completeSceneInitialization(true, currentScene.name);
		}

		public void Increment() {
			Instance.gameData.count++;

			Scene currentScene = SceneManager.GetActiveScene ();
			if (currentScene.name == "game_control_test02") {
				EventCenter.Instance.UpdateIntTask ("incrementCount", Instance.gameData.count);
			}
		}

		public bool GetFlag(string key) {
			return FlagDataUtils.GetByKey (key, Instance.gameData.flags).value;
		}

		public void SetFlag(string key, bool value) {
			FlagDataUtils.SetByKey (key, value, Instance.gameData.flags);
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

		private void _changeScene(string scene, int section = -1) {
			EventCenter.Instance.StartSceneChange (scene, section);
		}

		private void _prepForSceneChange(string scene, int section = -1) {
			Scene currentScene = SceneManager.GetActiveScene ();
			bool isLevel = _getIsLevel (currentScene.name);

			if (section > -1) {
				Instance.gameData.targetSection = section;
			}

			if (scene != currentScene.name) {
				if (isLevel) {
					if (_levelController != null) {
						LevelUtils.SetLevelData (currentScene.name, Instance.gameData.levels, _levelController.GetLevelData());
					}
					Instance.gameData.items = Instance.playerInventory.GetAll ();
					Instance.gameData.targetSection = section;
				}
				EventCenter.Instance.ContinueSceneChange(scene, section);
			}
		}

		private void _loadScene(string scene) {
			_cleanUp ();
			SceneManager.LoadScene (scene);
		}

		private void _initLevel(string currentSceneName, Hashtable items) {
			_levelController = GameObject.Find("level_controller").GetComponent<LevelController>();
			_levelController.Init (Instance.gameData);
		}

		private void _completeSceneInitialization(bool isLevel, string currentSceneName) {
			Instance.isSceneInitialized = true;
			EventCenter.Instance.SceneInitializationComplete (currentSceneName);

			InputManager inputManager = GetComponent<InputManager> ();
			inputManager.Init (isLevel);
		}

		private bool _getIsLevel(string sceneName) {
			return LevelUtils.Has (sceneName, Instance.gameData.levels);
		}

		private void _cleanUp() {
			_levelController = null;
			EventCenter ec = EventCenter.Instance;
			if (ec != null) {
				ec.OnStartSceneChange -= OnStartSceneChange;
				ec.OnCompleteSceneChange -= OnCompleteSceneChange;
			}
		}
		#endregion
	}
}
