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

//			TaskController taskController = GetComponent<TaskController> ();
//			taskController.Init (gameData.completedTasks);
		}

	}
}

