using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class SceneController : Singleton<SceneController>
	{
		[SerializeField] private SceneData sceneData;

		public void Init(GameData gameData) {
			ScenePrefabController scenePrefabController = GetComponent<ScenePrefabController> ();
			scenePrefabController.Init (sceneData.prefabs, gameData.items);

			if (gameData.clearedScenes [sceneData] == null) {
				TaskController taskController = GetComponent<TaskController> ();
				Hashtable taskData = gameData.tasks [sceneData.sceneName] as Hashtable;
				taskController.Init (sceneData, taskData);
			} else {
				Debug.Log ("SceneController/Init, scene already cleared");
			}

			EventCenter.Instance.OnSceneTasksCompleted += OnSceneTasksCompleted;
		}

		public void OnSceneTasksCompleted() {
			Debug.Log ("SceneController/OnSceneTasksCompleted");
			Game.Instance.gameData.clearedScenes.Add (sceneData.sceneName, true);
		}

		public SceneTaskData GetData() {
			TaskController taskController = GetComponent<TaskController> ();
			return taskController.GetData ();
		}
	}
}

