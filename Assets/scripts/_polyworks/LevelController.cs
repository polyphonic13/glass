using UnityEngine;
using System.Collections;

namespace Polyworks
{
	public class LevelController : Singleton<SceneController>
	{
		[SerializeField] private SceneData sceneData;

		public void Init(GameData gameData) {
//			ScenePrefabController scenePrefabController = GetComponent<ScenePrefabController> ();
			ScenePrefabController.Init (sceneData.prefabs, gameData.items);
			bool isCleared = LevelUtils.GetIsCleared (sceneData.sceneName, Game.Instance.gameData.levels);
			// Debug.Log ("LevelController/Init, isCleared = " + isCleared);
			if (!isCleared) {
				TaskController taskController = GetComponent<TaskController> ();
				LevelData level = LevelUtils.GetLevel (sceneData.sceneName, gameData.levels);
				if (level != null) {
					// Debug.Log (" level not null, going to init task controller");
					LevelTaskData taskData = level.tasks as LevelTaskData;
					taskController.Init (taskData);
				}
			} else {
				// Debug.Log (" scene already cleared");
			}

			EventCenter.Instance.OnLevelTasksCompleted += OnLevelTasksCompleted;
		}

		public void OnLevelTasksCompleted() {
			// Debug.Log ("LevelController/OnLevelTasksCompleted, sceneData.sceneName = " + sceneData.sceneName);
			LevelUtils.SetIsCleared (sceneData.sceneName, Game.Instance.gameData.levels);
		}

		public void Cleanup() {
			EventCenter.Instance.OnLevelTasksCompleted -= OnLevelTasksCompleted;

			TaskController taskController = GetComponent<TaskController> ();
			taskController.Cleanup ();
		}
	}
}

