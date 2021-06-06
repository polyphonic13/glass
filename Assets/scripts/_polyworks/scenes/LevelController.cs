namespace Polyworks
{
    using UnityEngine;

    public class LevelController : MonoBehaviour
    {
        public SectionController[] sectionControllers;

        private LevelData levelData;
        private PlayerManager playerManager;

        private GameData gameData;
        private SubSceneData subSceneData;

        #region handlers
        public void OnPrefabsAdded()
        {
            finishInitialization();
        }

        public void OnLevelTasksCompleted()
        {
            LevelUtils.SetIsCleared(subSceneData.name, gameData.levels);
        }

        public void OnSectionChanged(int section)
        {
            levelData.currentSection = section;
        }
        #endregion

        #region public methods
        public void Init(GameData gameData, SubSceneData subSceneData)
        {
            // Debug.Log ("LevelController/Init, gameData = " + gameData);
            this.gameData = gameData;
            this.subSceneData = subSceneData;

            addListeners();

            ScenePrefabController.Init(subSceneData.sections, gameData.items);
        }

        public LevelData GetLevelData()
        {
            return levelData;
        }
        #endregion

        #region private methods
        private void addListeners()
        {
            EventCenter eventCenter = EventCenter.Instance;
            eventCenter.OnPrefabsAdded += OnPrefabsAdded;
            eventCenter.OnLevelTasksCompleted += OnLevelTasksCompleted;
            eventCenter.OnSectionChanged += OnSectionChanged;
        }

        private void finishInitialization()
        {
            // Debug.Log("LevelController/finishInitialization, gameData = " + gameData.targetSection + ", sectionController = " + sectionControllers.Length);
            bool isCleared = LevelUtils.GetIsCleared(subSceneData.name, gameData.levels);
            levelData = LevelUtils.GetLevel(subSceneData.name, gameData.levels);

            if (gameData.targetSection > -1 && gameData.targetSection < sectionControllers.Length)
            {
                levelData.currentSection = gameData.targetSection;
            }

            if (sectionControllers != null)
            {
                foreach (SectionController sectionController in sectionControllers)
                {
                    sectionController.Init(levelData.currentSection);
                }

                PlayerLocation playerLocation = sectionControllers[levelData.currentSection].data.playerLocation;
                playerManager = GetComponent<PlayerManager>();
                playerManager.Init(playerLocation, gameData.playerData, gameData.items);
            }

            if (!isCleared)
            {
                TaskController taskController = GetComponent<TaskController>();
                if (levelData != null)
                {
                    LevelTaskData taskData = levelData.tasks as LevelTaskData;
                    taskController.Init(taskData);

                }
            }
            else
            {
                Debug.Log("LevelController[" + subSceneData.name + "]/Initlevel cleared");
            }

            Game.Instance.LevelInitialized();
        }

        private void OnDestroy()
        {
            EventCenter ec = EventCenter.Instance;
            if (ec != null)
            {
                ec.OnPrefabsAdded -= OnPrefabsAdded;
                ec.OnLevelTasksCompleted -= OnLevelTasksCompleted;
                ec.OnSectionChanged -= OnSectionChanged;
            }
        }
        #endregion
    }
}

