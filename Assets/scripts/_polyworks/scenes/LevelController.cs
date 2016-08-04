using UnityEngine;
using System.Collections;

namespace Polyworks
{
	public class LevelController : MonoBehaviour
	{
		public SceneData sceneData;

		private LevelData _levelData; 
		private PlayerManager _playerManager;

		#region handlers
		public void OnLevelTasksCompleted() {
			LevelUtils.SetIsCleared (sceneData.sceneName, Game.Instance.gameData.levels);
		}

		public void OnSectionChanged(int section) {
			_levelData.currentSection = section;
		}
		#endregion

		#region public methods
		public void Init(GameData gameData) {
			ScenePrefabController.Init (sceneData.prefabs, gameData.items);
			bool isCleared = LevelUtils.GetIsCleared (sceneData.sceneName, Game.Instance.gameData.levels);
			_levelData = LevelUtils.GetLevel (sceneData.sceneName, gameData.levels);

			if (gameData.targetSection > -1) {
				_levelData.currentSection = gameData.targetSection;
			}

			if (!isCleared) {
				TaskController taskController = GetComponent<TaskController> ();
				if (_levelData != null) {
					LevelTaskData taskData = _levelData.tasks as LevelTaskData;
					taskController.Init (taskData);

				}
			} else {
				Debug.Log ("LevelController["+sceneData.sceneName+"]/Initlevel cleared");
			}

			PlayerLocation playerLocation = sceneData.sections[_levelData.currentSection].playerLocation;
			_playerManager = GetComponent<PlayerManager> ();
			_playerManager.Init (playerLocation, gameData.playerData, gameData.items);

			EventCenter ec = EventCenter.Instance;
			ec.ChangeSection (_levelData.currentSection);
			ec.OnLevelTasksCompleted += OnLevelTasksCompleted;
			ec.OnSectionChanged += OnSectionChanged;

			Game.Instance.LevelInitialized ();
		}

		public LevelData GetLevelData() {
			return _levelData;
		}
		#endregion
	}
}

