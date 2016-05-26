using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Reflection;
using System;

namespace Polyworks {
	public class GameController : MonoBehaviour {

		public GameData gameData;

		public string dataFilename = "game_data.dat"; 

		public string loadingScene = ""; 
		public int loadingScenePause = 2;

		public bool isCursorless = true;

		public string[] playerScenes;

		public CompletedTaskController[] tasks;

		private DataIOController _dataIOController; 

		public static GameController Instance;

		public virtual void Init() {
			Scene currentScene = SceneManager.GetActiveScene ();

			_dataIOController = new DataIOController ();
//			Load ();

			if (isCursorless) {
				Cursor.visible = false;
			}
			Debug.Log ("currentScene.name = " + currentScene.name + "Instance.loadingScene = " + Instance.loadingScene + "Instance.gameData.currentTargetScene = " + Instance.gameData.currentTargetScene);
			if (currentScene.name == Instance.loadingScene && Instance.gameData.currentTargetScene != "") {
				Instance.StartCoroutine(_pauseDuringLoading());
			} else {
				if (Instance.gameData.items == null) {
					Instance.gameData.items = new Hashtable ();
				}

				Instance.gameData.currentScene = currentScene.name; 

				for (int i = 0; i < Instance.playerScenes.Length; i++) {
					if(Instance.playerScenes[i] == currentScene.name) {
						_initPlayerScene(currentScene.name);
						break;
					}
				}
			}
			EventCenter.Instance.OnChangeScene += OnChangeScene;
			Iterate (Instance.gameData, "count", 14);
		}

		public void Iterate(object p, string propName, object value) {
			Type type = p.GetType ();
			Debug.Log ("Iterate/propName = " + propName + ", value = " + value + ", type = " + type);
			foreach (PropertyInfo info in type.GetProperties()) {
				Debug.Log ("name = " + info.Name);
				if (info.Name == propName && info.CanWrite) {
//					info.SetValue (p, value, null);
					Debug.Log("writable name match on " + info.Name);
				}
			}
		}

		public void Save() {
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

			string toLoad = Instance.gameData.currentTargetScene;
			Debug.Log ("toLoad = " + toLoad);
			Instance.gameData.currentTargetScene = "";
			_loadScene (toLoad);
		}

		private void _initPlayerScene(string currentSceneName) {
			Inventory.Instance.Init (Instance.gameData.items);

			ScenePrefabData scenePrefabData = GetComponent<ScenePrefabData> ();
			if (scenePrefabData != null && scenePrefabData.prefabs.Length > 0) {
				_initScenePrefabs (scenePrefabData.prefabs, currentSceneName);
			}
		}

		private void _initScenePrefabs(Prefab[] prefabs, string currentScene) {
			for (int i = 0; i < prefabs.Length; i++) {
				Debug.Log ("prefabs ["+i+"].name = " + prefabs [i].name);
				bool isAddable = true; 

				if (Instance.gameData.items.Contains (prefabs [i].name)) {
					Debug.Log ("gameData.items contains " + prefabs [i].name);
					isAddable = false;
				}

				if(isAddable) {
					Debug.Log ("isAddable: " + isAddable);
					GameObject go = (GameObject) Instantiate (Resources.Load (prefabs [i].name, typeof(GameObject)), prefabs [i].location, prefabs [i].rotation);
				}
			}
		}

		private void _loadScene(string scene) {
			SceneManager.LoadScene (scene);
		}

		public void ChangeScene(string scene) {
			Debug.Log ("GameController/ChangeScene, scene = " + scene + ", gameData.items.Count = " + Instance.gameData.items.Count);
			Scene currentScene = SceneManager.GetActiveScene ();

			if (scene != currentScene.name) {
				Instance.gameData.items = Inventory.Instance.GetAll ();
				Instance.gameData.currentTargetScene = scene;

				if (Instance.loadingScene != "") {
					_loadScene (Instance.loadingScene);
				} else {
					_loadScene (scene);
				}
			}
		}

		public void OnChangeScene(string scene) {
//			Debug.Log ("GameController/OnChangeScene, scene = " + scene);
			ChangeScene (scene);
		}

		public void Increment() {
			Instance.gameData.count++;
		}
	}
}
