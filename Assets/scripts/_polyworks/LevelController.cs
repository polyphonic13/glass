using UnityEngine;
using System.Collections;

namespace Polyworks
{
	public class LevelController : Singleton<LevelController>
	{
		[SerializeField] private SceneData sceneData;

		private PlayerManager _playerManager;

		public void Init(GameData gameData) {
			ScenePrefabController.Init (sceneData.prefabs, gameData.items);
			bool isCleared = LevelUtils.GetIsCleared (sceneData.sceneName, Game.Instance.gameData.levels);

			if (!isCleared) {
				TaskController taskController = GetComponent<TaskController> ();
				LevelData level = LevelUtils.GetLevel (sceneData.sceneName, gameData.levels);
				if (level != null) {
					LevelTaskData taskData = level.tasks as LevelTaskData;
					taskController.Init (taskData);

					PlayerLocation playerLocation = sceneData.playerLocations [level.currentPlayerLocation];
					_playerManager = GetComponent<PlayerManager> ();
					_playerManager.Init (playerLocation, gameData.playerData, gameData.items);
				}
			} else {
				Debug.Log (" level already cleared");
			}
			EventCenter ec = EventCenter.Instance;
			ec.OnLevelTasksCompleted += OnLevelTasksCompleted;

			Game.Instance.LevelInitialized (this);
		}

		public void OnLevelTasksCompleted() {
			LevelUtils.SetIsCleared (sceneData.sceneName, Game.Instance.gameData.levels);
		}

		public Player GetPlayer() {
			if (_playerManager == null) {
				return null;
			}
			return _playerManager.GetPlayer();
		}

		public Inventory GetPlayerInventory() {
			if (_playerManager == null) {
				return null;
			}
			return _playerManager.GetInventory ();
		}

		public void Cleanup() {
			EventCenter.Instance.OnLevelTasksCompleted -= OnLevelTasksCompleted;

			TaskController taskController = GetComponent<TaskController> ();
			taskController.Cleanup ();
		}
	}
}

