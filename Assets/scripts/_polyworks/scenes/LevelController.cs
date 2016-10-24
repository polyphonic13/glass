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

		private GameData _gameData;

		#region handlers
		public void OnPrefabsAdded() {
			_finishInitialization ();		
		}

		public void OnLevelTasksCompleted() {
			LevelUtils.SetIsCleared (sceneData.sceneName, Game.Instance.gameData.levels);
		}

		public void OnSectionChanged(int section) {
			_levelData.currentSection = section;
		}
		#endregion

		#region public methods
		public void Init(GameData gameData) {
			_gameData = gameData;
			EventCenter ec = EventCenter.Instance;
			ec.OnPrefabsAdded += OnPrefabsAdded;
			ec.OnLevelTasksCompleted += OnLevelTasksCompleted;
			ec.OnSectionChanged += OnSectionChanged;
			ScenePrefabController.Init (sceneData.sectionPrefabs, gameData.items);
		}

		public LevelData GetLevelData() {
			return _levelData;
		}
		#endregion

		#region private methods
		private void _finishInitialization() {
			bool isCleared = LevelUtils.GetIsCleared (sceneData.sceneName, Game.Instance.gameData.levels);
			_levelData = LevelUtils.GetLevel (sceneData.sceneName, _gameData.levels);
			if (_gameData.targetSection > -1 && _gameData.targetSection < sectionControllers.Length) {
				_levelData.currentSection = _gameData.targetSection;
			}

			if (sectionControllers != null) {
				foreach (SectionController sectionController in sectionControllers) {
					sectionController.Init (_levelData.currentSection);
				}

				PlayerLocation playerLocation = sectionControllers [_levelData.currentSection].data.playerLocation;
				_playerManager = GetComponent<PlayerManager> ();
				_playerManager.Init (playerLocation, _gameData.playerData, _gameData.items);
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

			Game.Instance.LevelInitialized ();
		}

		private void OnDestroy() {
			EventCenter ec = EventCenter.Instance;
			if (ec != null) {
				ec.OnPrefabsAdded -= OnPrefabsAdded;
				ec.OnLevelTasksCompleted -= OnLevelTasksCompleted;
				ec.OnSectionChanged -= OnSectionChanged;
			}
		}
		#endregion
	}
}

