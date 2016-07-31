using UnityEngine;
using System.Collections;

namespace Polyworks
{
	public class LevelController : MonoBehaviour
	{
		public SceneData sceneData;
		public LevelData levelData; 

		private PlayerManager _playerManager;

		#region handlers
		public void OnLevelTasksCompleted() {
			LevelUtils.SetIsCleared (sceneData.sceneName, Game.Instance.gameData.levels);
		}

		public void OnSectionChanged(int section) {
			levelData.currentSection = section;
		}
		#endregion

		#region public methods
		public void Init(GameData gameData) {
			ScenePrefabController.Init (sceneData.prefabs, gameData.items);
			bool isCleared = LevelUtils.GetIsCleared (sceneData.sceneName, Game.Instance.gameData.levels);
			levelData = LevelUtils.GetLevel (sceneData.sceneName, gameData.levels);

			if (gameData.targetSection > -1) {
				levelData.currentSection = gameData.targetSection;
			}

			if (!isCleared) {
				TaskController taskController = GetComponent<TaskController> ();
				if (levelData != null) {
					LevelTaskData taskData = levelData.tasks as LevelTaskData;
					taskController.Init (taskData);

				}
			} else {
				Debug.Log ("LevelController["+sceneData.sceneName+"]/Initlevel cleared");
			}

			PlayerLocation playerLocation = sceneData.playerLocations [levelData.currentSection];
			_playerManager = GetComponent<PlayerManager> ();
			_playerManager.Init (playerLocation, gameData.playerData, gameData.items);

			EventCenter ec = EventCenter.Instance;
			ec.ChangeSection (levelData.currentSection);
			ec.OnLevelTasksCompleted += OnLevelTasksCompleted;
			ec.OnSectionChanged += OnSectionChanged;

			Game.Instance.LevelInitialized ();
		}

		public Player GetPlayer() {
			if (_playerManager == null) {
				return null;
			}
			return _playerManager.GetPlayer();
		}

		#endregion
	}
}

