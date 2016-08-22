using UnityEngine;
using System.Collections;

namespace Polyworks
{
	public class LevelController : MonoBehaviour
	{
		public SceneData sceneData;
		public SectionController[] sectionControllers; 

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
			ScenePrefabController.Init (sceneData.sectionPrefabs, gameData.items);

			bool isCleared = LevelUtils.GetIsCleared (sceneData.sceneName, Game.Instance.gameData.levels);
			_levelData = LevelUtils.GetLevel (sceneData.sceneName, gameData.levels);

			if (gameData.targetSection > -1 && gameData.targetSection < sectionControllers.Length) {
				_levelData.currentSection = gameData.targetSection;
			}

			if (sectionControllers != null) {
				foreach (SectionController sectionController in sectionControllers) {
					sectionController.Init (_levelData.currentSection);
				}

				PlayerLocation playerLocation = sectionControllers [_levelData.currentSection].data.playerLocation;
				_playerManager = GetComponent<PlayerManager> ();
				_playerManager.Init (playerLocation, gameData.playerData, gameData.items);
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

			EventCenter ec = EventCenter.Instance;
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

