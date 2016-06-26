﻿using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class SceneController : Singleton<SceneController>
	{
		[SerializeField] private SceneData sceneData;

		public void Init(GameData gameData) {
//			ScenePrefabController scenePrefabController = GetComponent<ScenePrefabController> ();
			ScenePrefabController.Init (sceneData.prefabs, gameData.items);
			bool isCleared = LevelUtils.GetIsCleared (sceneData.sceneName, Game.Instance.gameData.levels);
			// Debug.Log ("SceneController/Init, isCleared = " + isCleared);
			if (!isCleared) {
				TaskController taskController = GetComponent<TaskController> ();
				Hashtable taskData = gameData.tasks [sceneData.sceneName] as Hashtable;
//				taskController.Init (sceneData, taskData);
			} else {
				// Debug.Log (" scene already cleared");
			}

			EventCenter.Instance.OnLevelTasksCompleted += OnLevelTasksCompleted;
		}

		public void OnLevelTasksCompleted() {
			// Debug.Log ("SceneController/OnLevelTasksCompleted");
			LevelUtils.SetIsCleared (sceneData.sceneName, Game.Instance.gameData.levels);
		}

//		public LevelTaskData GetData() {
//			TaskController taskController = GetComponent<TaskController> ();
//			return taskController.GetData ();
//		}
	}
}

