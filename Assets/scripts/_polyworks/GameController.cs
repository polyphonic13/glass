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
				DontDestroyOnLoad(gameObject);
				Instance = this;
			} else if(this != Instance) {
				Destroy(gameObject);
			}
			Init ();
		}

		public virtual void Init() {
			if (isCursorless) {
				Cursor.visible = false;
			}

			Scene currentScene = SceneManager.GetActiveScene ();

			if (currentScene.name == Instance.loadingScene && Instance.gameData.currentTargetScene != "") {
				Instance.StartCoroutine(_pauseDuringLoading());
			} else {
				Debug.Log ("currentScene = " + currentScene);
				for (int i = 0; i < Instance.playerScenes.Length; i++) {
					Debug.Log ("playerScenes[" + i + "] = " + playerScenes [i]);
					if(Instance.playerScenes[i] == currentScene.name) {
						_initPlayerScene();
						break;
					}
				}
			}
			EventCenter.Instance.OnChangeScene += OnChangeScene;
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
			Instance.gameData.items = Inventory.Instance.GetAll();
			Debug.Log ("gameData.items.Count = " + Instance.gameData.items.Count);
			Instance.gameData.currentTargetScene = scene;

			if (Instance.loadingScene != "") {
				_loadScene (Instance.loadingScene);
			} else {
				_loadScene (scene);
			}
		}

		public void OnChangeScene(string scene) {
			ChangeScene (scene);
		}
	}
}
