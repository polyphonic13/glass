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

			GoalController goalController = GetComponent<GoalController> ();

		}

		private void _initTasks(GameData gameData) {
			if (gameData.tasks [sceneData.sceneName] == null) {
				Hashtable taskData = new Hashtable ();
				taskData.Add("countTasks", sceneData.countTasks);
				taskData.Add("valueTasks", sceneData.valueTasks);
				taskData.Add("goalTasks",  sceneData.goalTasks);

				gameData.tasks [sceneData.sceneName] = taskData;
			} else {
				Hashtable taskData = gameData.tasks [sceneData.sceneName] as Hashtable;
				sceneData.countTasks = taskData["countTasks"] as CountTaskData[];
				sceneData.valueTasks = taskData["valueTasks"] as ValueTaskData[];
				sceneData.goalTasks = taskData["goalTasks"] as GoalTaskData[];
			}

			TaskController taskController = GetComponent<TaskController> ();
			taskController.Init (sceneData);

		}
	}
}

