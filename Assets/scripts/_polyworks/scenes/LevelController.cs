using UnityEngine;
using System.Collections;

namespace Polyworks
{
	public class LevelController : Singleton<LevelController>
	{
		public SceneData sceneData;

		private PlayerManager _playerManager;
		public LevelData levelData; 

		#region handlers
		public void OnLevelTasksCompleted() {
			LevelUtils.SetIsCleared (sceneData.sceneName, Game.Instance.gameData.levels);
		}

		public void OnSectionChanged(int section) {
			Instance.levelData.currentSection = section;
		}
		#endregion

		#region public methods
		public void Init(GameData gameData) {
			ScenePrefabController.Init (sceneData.prefabs, gameData.items);
			bool isCleared = LevelUtils.GetIsCleared (sceneData.sceneName, Game.Instance.gameData.levels);
			Instance.levelData = LevelUtils.GetLevel (sceneData.sceneName, gameData.levels);
			if (gameData.targetSection > -1) {
				Instance.levelData.currentSection = gameData.targetSection;
			}

			if (!isCleared) {
				TaskController taskController = GetComponent<TaskController> ();
				if (Instance.levelData != null) {
					LevelTaskData taskData = Instance.levelData.tasks as LevelTaskData;
					taskController.Init (taskData);

				}
			} else {
				Debug.Log ("LevelController["+sceneData.sceneName+"]/Initlevel cleared");
			}

			PlayerLocation playerLocation = sceneData.playerLocations [Instance.levelData.currentSection];
			_playerManager = GetComponent<PlayerManager> ();
			_playerManager.Init (playerLocation, gameData.playerData, gameData.items);

			EventCenter ec = EventCenter.Instance;
			ec.ChangeSection (Instance.levelData.currentSection);
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

		public Inventory GetPlayerInventory() {
			if (_playerManager == null) {
				return null;
			}
			return _playerManager.GetInventory ();
		}

		public LevelData GetLevelData() {
			return Instance.levelData;
		}

		public void OnDestroy() {
			EventCenter ec = EventCenter.Instance;
			if (ec != null) {
				ec.OnLevelTasksCompleted -= OnLevelTasksCompleted;
				ec.OnSectionChanged -= OnSectionChanged;
			}
		}
		#endregion
	}
}

