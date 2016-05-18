using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Polyworks {
	public class GameController : MonoBehaviour {

		public GameData gameData;

		public string loadingScene = ""; 
		public int loadingScenePause = 2;

		public bool isCursorless = true;

		public string[] playerScenes;

		private DataIOController _dataIOController; 

		public static GameController Instance;

		void Awake() {
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

		public virtual void Init() {
			Scene currentScene = SceneManager.GetActiveScene ();

			if (isCursorless) {
				Cursor.visible = false;
			}

			if (Instance.gameData.items == null) {
				Instance.gameData.items = new Hashtable ();
			} else {
				foreach (ItemData item in Instance.gameData.items.Values) {
					Debug.Log ("Item: " + item.itemName);
				}
			}

			if (Instance.gameData.scenePrefabs.Length > 0) {
				_findScenePrefabCollection (Instance.gameData.scenePrefabs, currentScene.name);
			}


			if (currentScene.name == Instance.loadingScene && Instance.gameData.currentTargetScene != "") {
				Instance.StartCoroutine(_pauseDuringLoading());
			} else {
				Debug.Log ("currentScene = " + currentScene.name);
				for (int i = 0; i < Instance.playerScenes.Length; i++) {
//					Debug.Log ("playerScenes[" + i + "] = " + playerScenes [i]);
					if(Instance.playerScenes[i] == currentScene.name) {
						_initPlayerScene();
						break;
					}
				}
			}
			EventCenter.Instance.OnChangeScene += OnChangeScene;
		}

		private void _findScenePrefabCollection(ScenePrefabCollection[] collection, string currentScene) {
			ScenePrefabCollection scenePrefabs;
			int i;

			for (i = 0; i < collection.Length; i++) {
				if (collection[i].sceneName == currentScene) {
					scenePrefabs = collection[i];
					if (scenePrefabs.prefabs.Length > 0) {
						Prefab[] prefabs = scenePrefabs.prefabs;
						for (i = 0; i < prefabs.Length; i++) {
							Debug.Log ("prefabs ["+i+"].name = " + prefabs [i].name);
							bool isAddable = true; 

							if (Instance.gameData.items.Contains (prefabs [i].name)) {
								Debug.Log ("gameData.items contains " + prefabs [i].name);
								isAddable = false;
//								Item gameDataItem = Instance.gameData.items [prefabs [i]] as Item;
//								if (gameDataItem.Data.isCollected) {
//									isAddable = false;
//								}
							}

							if(isAddable) {
								Debug.Log ("isAddable: " + isAddable);
								GameObject go = Instantiate (Resources.Load (prefabs [i].name, typeof(GameObject)), prefabs [i].location, prefabs [i].rotation) as GameObject;
//								Item item = go.GetComponent<Item> ();
//								if (item != null) {
//	
//								}
							}
						}
					}
				}
			}

		}

		private IEnumerator _pauseDuringLoading() {
			yield return new WaitForSeconds (Instance.loadingScenePause);
			string toLoad = Instance.gameData.currentTargetScene;
			Instance.gameData.currentTargetScene = "";
			_loadScene (toLoad);
		}

		private void _initPlayerScene() {
			Inventory.Instance.Init (Instance.gameData.items);

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
