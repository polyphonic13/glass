using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class SceneController : Singleton<SceneController>
	{
		[SerializeField] private SceneData sceneData;

		public void Init(GameData gameData) {
			Debug.Log ("SceneController/Init");

			ScenePrefabController scenePrefabController = GetComponent<ScenePrefabController> ();
			scenePrefabController.Init (sceneData.prefabs, gameData.items);

			if (gameData.clearedScenes [sceneData] == null && gameData.tasks[sceneData.sceneName] != null) {
				TaskController taskController = GetComponent<TaskController> ();
				Hashtable taskData = gameData.tasks [sceneData.sceneName] as Hashtable;
				taskController.Init (sceneData, taskData);
			}

			EventCenter.Instance.OnSceneTasksCompleted += OnSceneTasksCompleted;
		}

		public void OnSceneTasksCompleted() {
			Game.Instance.gameData.clearedScenes.Add (sceneData.sceneName, true);
		}

		public Hashtable GetData() {
			TaskController taskController = GetComponent<TaskController> ();
			return taskController.GetData ();
		}
	}
}

