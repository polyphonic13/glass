using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class SceneController : Singleton<SceneController>
	{
		[SerializeField] private SceneData sceneData;

		public void Init(GameData gameData) {

			TaskController taskController = GetComponent<TaskController> ();
			taskController.Init (gameData.completedTasks);

			ScenePrefabController scenePrefabController = GetComponent<ScenePrefabController> ();
			scenePrefabController.Init (sceneData.prefabs, gameData.items);
		}

	}
}

