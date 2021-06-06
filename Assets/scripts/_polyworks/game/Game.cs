namespace Polyworks
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using System.Collections;

    [RequireComponent(typeof(EventCenter))]
    [RequireComponent(typeof(InputManager))]
    [RequireComponent(typeof(SceneChanger))]
    [RequireComponent(typeof(SceneController))]
    [RequireComponent(typeof(ItemUtils))]
    [RequireComponent(typeof(Utilities))]
    public class Game : MonoBehaviour
    {
        public static Game Instance;
        public GameData gameData;
        public string playerPrefab = "player_objects";
        public bool isSceneInitialized = false;
        public bool isCursorless = true;
        public Inventory playerInventory { get; set; }

        private static readonly string DATA_FILE_NAME = "game_data.dat";
        private static readonly string INVENTORY_GAME_OBJECT = "inventory_ui";
        private static readonly string LEVEL_CONTROLLER_GAME_OBJECT = "level_controller";

        private EventCenter eventCenter;
        private LevelController levelController;
        private Player player;
        private DataIOController dataIOController;
        private string dataPath;


        #region handlers
        public void OnStartSceneChange(string scene, int section = -1)
        {
            prepForSceneChange(scene, section);
        }

        public void OnCompleteSceneChange(string scene, int section = -1)
        {
            loadScene(scene);
        }

        #endregion

        #region public methods
        public virtual Inventory GetPlayerInventory()
        {
            return Instance.playerInventory;
        }


        public void LevelInitialized()
        {
            PlayerManager pm = GameObject.Find(LEVEL_CONTROLLER_GAME_OBJECT).GetComponent<PlayerManager>();
            player = pm.GetPlayer();
            Instance.playerInventory = pm.GetInventory();
            Scene currentScene = SceneManager.GetActiveScene();

            GameObject inventoryObj = GameObject.Find("inventory_ui");
            if (inventoryObj != null)
            {
                InventoryUI inventoryUI = inventoryObj.GetComponent<InventoryUI>();
                inventoryUI.InitInventory(Instance.playerInventory);
            }
            NotificationUIController.Instance.Init();

            completeSceneInitialization(true, currentScene.name);
        }

        public bool GetFlag(string key)
        {
            return FlagDataUtils.GetByKey(key, Instance.gameData.flags).value;
        }

        public void SetFlag(string key, bool value)
        {
            Debug.Log("Game/SetFlag, key = " + key + ", value = " + value);
            FlagDataUtils.SetByKey(key, value, Instance.gameData.flags);
            saveData();
        }
        #endregion

        #region private methods
        public virtual void init()
        {
            eventCenter = EventCenter.Instance;
            Scene currentScene = SceneManager.GetActiveScene();
            string currentSceneName = currentScene.name;
            bool isLevel = getIsLevel(currentSceneName);
            // Debug.Log("Game/Init, isLevel = " + isLevel);
            Instance.isSceneInitialized = false;

            dataPath = Application.persistentDataPath + "/" + DATA_FILE_NAME;
            dataIOController = new DataIOController();

            Cursor.visible = (isCursorless) ? false : true;

            Instance.gameData = initGameData();

            Instance.gameData.currentScene = currentSceneName;
            Hashtable items = Instance.gameData.items;

            SceneChanger.Instance.Init(currentSceneName);

            eventCenter.OnStartSceneChange += OnStartSceneChange;
            eventCenter.OnCompleteSceneChange += OnCompleteSceneChange;

            if (isLevel)
            {
                initLevel(currentSceneName, items);
                return;
            }
            completeSceneInitialization(isLevel, currentSceneName);
        }

        private GameData initGameData()
        {
            GameData data = Instance.gameData;

            data.tasks = (data.tasks != null) ? data.tasks : new Hashtable();
            data.items = (data.items != null) ? data.items : new Hashtable();
            data.clearedScenes = (data.clearedScenes != null) ? data.clearedScenes : new Hashtable();

            return data;
        }

        private void initInventory()
        {
            GameObject inventoryObj = GameObject.Find(INVENTORY_GAME_OBJECT);
            if (inventoryObj == null)
            {
                return;
            }
            InventoryUI inventoryUI = inventoryObj.GetComponent<InventoryUI>();
            inventoryUI.InitInventory(Instance.playerInventory);
        }

        public void loadData()
        {
            GameData data = dataIOController.Load(dataPath);
            if (data != null)
            {
                Instance.gameData = data;

                if (data.currentScene != "")
                {
                    changeScene(data.currentScene, data.targetSection);
                }
                else
                {
                    Scene currentScene = SceneManager.GetActiveScene();
                    string currentSceneName = currentScene.name;
                    changeScene(currentSceneName, data.targetSection);
                }
            }
        }

        private void saveData()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            Instance.gameData.items = Instance.playerInventory.GetAll();
            dataIOController.Save(dataPath, Instance.gameData);
        }

        private void changeScene(string scene, int section = -1)
        {
            eventCenter.StartSceneChange(scene, section);
        }

        private void prepForSceneChange(string scene, int section = -1)
        {
            Scene currentScene = SceneManager.GetActiveScene();

            if (section > -1)
            {
                Instance.gameData.targetSection = section;
            }

            if (scene == currentScene.name)
            {
                return;
            }
            wrapUpLevel(currentScene.name, section);
            eventCenter.ContinueSceneChange(scene, section);
        }

        private void wrapUpLevel(string currentSceneName, int section)
        {
            bool isLevel = getIsLevel(currentSceneName);
            if (!isLevel)
            {
                return;
            }

            Instance.gameData.items = Instance.playerInventory.GetAll();
            Instance.gameData.targetSection = section;

            if (levelController == null)
            {
                return;
            }
            LevelUtils.SetLevelData(currentSceneName, Instance.gameData.levels, levelController.GetLevelData());
        }

        private void loadScene(string scene)
        {
            cleanUp();
            SceneManager.LoadScene(scene);
        }

        private void initLevel(string currentSceneName, Hashtable items)
        {
            GameObject levelControllerGO = GameObject.Find(LEVEL_CONTROLLER_GAME_OBJECT);
            if (levelControllerGO == null)
            {
                Debug.LogError("ERROR: Level Controller game object could not be found");
                return;
            }
            levelController = levelControllerGO.GetComponent<LevelController>();
            levelController.Init(Instance.gameData);
        }

        private void completeSceneInitialization(bool isLevel, string currentSceneName)
        {
            Instance.isSceneInitialized = true;
            eventCenter.SceneInitializationComplete(currentSceneName);

            InputManager inputManager = GetComponent<InputManager>();
            inputManager.Init(isLevel);
        }

        private bool getIsLevel(string sceneName)
        {
            return LevelUtils.Has(sceneName, Instance.gameData.levels);
        }

        private void cleanUp()
        {
            levelController = null;

            if (eventCenter == null)
            {
                return;
            }
            eventCenter.OnStartSceneChange -= OnStartSceneChange;
            eventCenter.OnCompleteSceneChange -= OnCompleteSceneChange;
        }
        #endregion

        #region unity methods
        private void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }
            else if (this != Instance)
            {
                Destroy(gameObject);
            }
            init();
        }
        #endregion
    }
}
