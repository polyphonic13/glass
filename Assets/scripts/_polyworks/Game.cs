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

		public string[] playerScenes;

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
				Debug.Log ("have to make a new hashtable for Instance.gameData.items");
				Instance.gameData.items = new Hashtable ();
			}

			if (Instance.gameData.clearedScenes == null) {
				Instance.gameData.clearedScenes = new Hashtable ();
			}

			Instance.gameData.currentScene = currentSceneName; 

			if (_getIsPlayerScene (currentSceneName)) {
				_initPlayerScene (currentSceneName);
			} else {
				EventCenter.Instance.SceneInitializationComplete (currentSceneName);
			}
			EventCenter.Instance.OnChangeScene += OnChangeScene;
		}

		public void Save() {
			Scene currentScene = SceneManager.GetActiveScene ();
			Instance.gameData.items = Inventory.Instance.GetAll ();
			Debug.Log ("Game/Save, item count = " + Instance.gameData.items.Count);
			if (_getIsPlayerScene (currentScene.name)) {
				SceneController sceneController = GameObject.Find("scene_controller").GetComponent<SceneController> ();
				Instance.gameData.tasks [currentScene.name] = sceneController.GetData ();
			}
			_dataIOController.Save (Application.persistentDataPath + "/" + dataFilename, Instance.gameData);
		}

		public void Load() {
			GameData data = _dataIOController.Load (Application.persistentDataPath + "/" + dataFilename);
			Debug.Log ("post load");
			if (data != null) {
				Instance.gameData = data;
				Debug.Log ("loaded count = " + data.count + ", items  = " + data.items.Count);
//				Scene currentScene = SceneManager.GetActiveScene ();
//				string currentSceneName = currentScene.name;
//				if (data.currentScene != "" && data.currentScene != currentSceneName) {
//					Debug.Log ("switching to last scene");
//					ChangeScene (data.currentScene);
//				} else if (_getIsPlayerScene (currentSceneName)) {
//					Debug.Log ("(re)initializing player scene, items count = " + Instance.gameData.items.Count);
//					_initPlayerScene (currentSceneName);
//				}
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
			Debug.Log ("Game/ChangeScene, scene = " + scene + ", gameData.items.Count = " + Instance.gameData.items.Count);
			Scene currentScene = SceneManager.GetActiveScene ();

			if (scene != currentScene.name) {
//				Instance.gameData.items = Inventory.Instance.GetAll ();
				Instance.currentTargetScene = scene;

				SceneController sceneController = GameObject.Find ("scene_controller").GetComponent<SceneController> ();
				Instance.gameData.tasks [currentScene.name] = sceneController.GetData ();

				_loadScene (scene);
			} else if (_getIsPlayerScene (currentScene.name)) {
				_initPlayerScene (currentScene.name);
			}
		}

		public void OnChangeScene(string scene) {
			ChangeScene (scene);
		}

		public void Increment() {
			Instance.gameData.count++;

			Scene currentScene = SceneManager.GetActiveScene ();
			if (currentScene.name == "game_control_test02") {
				EventCenter.Instance.UpdateIntTask ("countTest", Instance.gameData.count);
			}
		}

		private void Awake() {
			if(Instance == null) {
				Debug.Log ("THIS is the instance");
				DontDestroyOnLoad(gameObject);
				Instance = this;
			} else if(this != Instance) {
				Debug.Log ("this is NOT the instance, items count = ");
				Destroy(gameObject);
			}
			Init ();
		}

		private void _initPlayerScene(string currentSceneName) {
			Hashtable items = Instance.gameData.items;
			Inventory.Instance.Init (items);

			SceneController sceneController = GameObject.Find("scene_controller").GetComponent<SceneController> ();
			sceneController.Init (Instance.gameData);

			EventCenter.Instance.SceneInitializationComplete (currentSceneName);
		}

		private void _loadScene(string scene) {
			SceneManager.LoadScene (scene);
		}

		private bool _getIsPlayerScene(string sceneName) {
			string[] playerScenes = Instance.playerScenes;
			for (int i = 0; i < playerScenes.Length; i++) {
				if (sceneName == playerScenes [i]) {
					return true;
				}
			}
			return false;
		}
	}
}
