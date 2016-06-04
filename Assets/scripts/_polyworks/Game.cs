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
		public string loadingScene = ""; 
		public int loadingScenePause = 2;

		public bool isCursorless = true;

		public string[] playerScenes;

		private DataIOController _dataIOController; 

		public static Game Instance;

		public virtual void Init() {
			Scene currentScene = SceneManager.GetActiveScene ();

			_dataIOController = new DataIOController ();

			if (isCursorless) {
				Cursor.visible = false;
			}
//			Debug.Log ("currentScene.name = " + currentScene.name + "Instance.loadingScene = " + Instance.loadingScene + "Instance.currentTargetScene= " + Instance.gameData.currentTargetScene);
			if (currentScene.name == Instance.loadingScene && Instance.currentTargetScene!= "") {
				Instance.StartCoroutine(_pauseDuringLoading());
			} else {
				if (Instance.gameData.tasks == null) {
					Instance.gameData.tasks = new Hashtable ();
				}

				if (Instance.gameData.items == null) {
					Instance.gameData.items = new Hashtable ();
				}

				if (Instance.gameData.clearedScenes == null) {
					Instance.gameData.clearedScenes = new Hashtable ();
				}

				Instance.gameData.currentScene = currentScene.name; 

				if(_getIsPlayerScene(currentScene.name)) {
					_initPlayerScene(currentScene.name);
				}
			}
			EventCenter.Instance.OnChangeScene += OnChangeScene;
//			Iterate (Instance.gameData, "count", 14);
		}

		public void Save() {
			Scene currentScene = SceneManager.GetActiveScene ();

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
				Scene currentScene = SceneManager.GetActiveScene ();
				if (data.currentScene != "" && data.currentScene != currentScene.name) {
					Debug.Log ("switching to last scene");
					ChangeScene (data.currentScene);
				}
			}
		}

		public void ChangeScene(string scene) {
			Debug.Log ("Game/ChangeScene, scene = " + scene + ", gameData.items.Count = " + Instance.gameData.items.Count);
			Scene currentScene = SceneManager.GetActiveScene ();

			if (scene != currentScene.name) {
				Instance.gameData.items = Inventory.Instance.GetAll ();
				Instance.currentTargetScene= scene;

				SceneController sceneController = GameObject.Find("scene_controller").GetComponent<SceneController> ();
				Instance.gameData.tasks [currentScene.name] = sceneController.GetData ();

				if (Instance.loadingScene != "") {
					_loadScene (Instance.loadingScene);
				} else {
					_loadScene (scene);
				}
			}
		}

		public void OnChangeScene(string scene) {
			//			Debug.Log ("Game/OnChangeScene, scene = " + scene);
			ChangeScene (scene);
		}

		public void Increment() {
			Instance.gameData.count++;
			EventCenter.Instance.UpdateIntTask ("countTest", Instance.gameData.count);
		}

		private void Awake() {
			if(Instance == null) {
				Debug.Log ("THIS is the instance");
				DontDestroyOnLoad(gameObject);
				Instance = this;
			} else if(this != Instance) {
				Debug.Log ("this is NOT the instance");
				Destroy(gameObject);
			}
			Init ();
		}

		private IEnumerator _pauseDuringLoading() {
			yield return new WaitForSeconds (Instance.loadingScenePause);

			string toLoad = Instance.currentTargetScene;
			Debug.Log ("toLoad = " + toLoad);
			Instance.currentTargetScene= "";
			_loadScene (toLoad);
		}

		private void _initPlayerScene(string currentSceneName) {
			Hashtable items = Instance.gameData.items;
//			Inventory.Instance.Init (items);

			SceneController sceneController = GameObject.Find("scene_controller").GetComponent<SceneController> ();
			sceneController.Init (Instance.gameData);
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
