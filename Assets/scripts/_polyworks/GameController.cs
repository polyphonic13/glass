using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Polyworks {
	public class GameController : MonoBehaviour {

		public GameData gameData;

		public string dataFilename = "game_data.dat"; 

		public string loadingScene = ""; 
		public int loadingScenePause = 2;

		public bool isCursorless = true;

		public string[] playerScenes;

		private DataIOController _dataIOController; 

		public static GameController Instance;

		public virtual void Init() {
			Scene currentScene = SceneManager.GetActiveScene ();

			_dataIOController = new DataIOController ();

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
		}

		public void Save() {
			Debug.Log ("pre save");
			_dataIOController.Save (Application.persistentDataPath + "/" + dataFilename, Instance.gameData);
			Debug.Log ("post save");
		}

		public void Load() {
			Instance.gameData = _dataIOController.Load (Application.persistentDataPath + "/" + dataFilename);
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
					GameObject go = Instantiate (Resources.Load (prefabs [i].name, typeof(GameObject)), prefabs [i].location, prefabs [i].rotation) as GameObject;
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
