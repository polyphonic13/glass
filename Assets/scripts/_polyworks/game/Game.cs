namespace Polyworks
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using System.Collections;

    [RequireComponent(typeof(EventCenter))]
    [RequireComponent(typeof(InputController))]
    [RequireComponent(typeof(SceneChanger))]
    [RequireComponent(typeof(SceneController))]
    [RequireComponent(typeof(ItemUtils))]
    [RequireComponent(typeof(Utilities))]
    public class Game : MonoBehaviour
    {
        #region members
        public static string PLAYER_PREFAB_NAME = "player/player_objects";

        public static Game Instance;
        public GameData gameData;
        public bool isSceneInitialized = false;
        public bool isCursorless = true;
        public Inventory playerInventory { get; set; }

        private static readonly string JSON_PATH = "data/game";
        private static readonly string DATA_FILE_NAME = "game_data.dat";
        private static readonly string REWIRED_INPUT_MANAGER_PATH = "game/";
        private static readonly string REWIRED_INPUT_MANAGER_NAME = "rewired_input_manager";
        private static readonly string INVENTORY_GAME_OBJECT = "inventory_ui";
        private static readonly string LEVEL_CONTROLLER_GAME_OBJECT = "level_controller";

        private EventCenter eventCenter;
        private SceneController subSceneController;
        private LevelController levelController;
        private Player player;
        private DataIOController dataIOController;
        private GameJSON gameJSON;
        private SceneType currentScene = SceneType.None;
        private SceneType previousScene = SceneType.None;
        private string dataPath;
        #endregion

        #region public event handlers
        public void OnChangeScene(SceneType type, bool isFadedOut)
        {
            previousScene = currentScene;
            currentScene = type;
            Debug.Log("Game/OnChangeScene, previousScene = " + previousScene + ", currentScene = " + currentScene);
            if (previousScene != SceneType.None)
            {
                return;
            }
            subSceneController.LoadSubScene(type, onSubSceneLoaded);
        }

        public void OnStartSceneChange(string subScene, int section = -1)
        {
            prepForSceneChange(subScene, section);
        }

        public void OnCompleteSceneChange(string subScene, int section = -1)
        {
            loadScene(subScene);
        }

        #endregion

        #region public methods
        public virtual Inventory GetPlayerInventory()
        {
            return Instance.playerInventory;
        }

        public ItemInspectionScale[] GetItemInspectionScales()
        {
            return gameJSON.itemInspectionScales;
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
            FlagDataUtils.SetByKey(key, value, Instance.gameData.flags);
            saveData();
        }
        #endregion

        #region private handlers
        private void onSubSceneUnloaded(bool isComplete)
        {
            subSceneController.LoadSubScene(currentScene, onSubSceneLoaded);
        }
        private void onSubSceneLoaded(bool isComplete)
        {
            initScene();
        }
        #endregion

        #region private methods
        public virtual void init()
        {
            subSceneController = GetComponent<SceneController>();

            string path = REWIRED_INPUT_MANAGER_PATH + REWIRED_INPUT_MANAGER_NAME;
            GameObject rewiredInputController = (GameObject)Instantiate(Resources.Load(path));
            rewiredInputController.name = REWIRED_INPUT_MANAGER_NAME;
            rewiredInputController.transform.SetParent(transform);

            Scene currentScene = SceneManager.GetActiveScene();
            string currentSceneName = currentScene.name;
            bool isLevel = getIsLevel(currentSceneName);
            // Debug.Log("Game/Init, isLevel = " + isLevel);
            Instance.isSceneInitialized = false;

            dataPath = Application.persistentDataPath + "/" + DATA_FILE_NAME;
            dataIOController = new DataIOController();

            Cursor.visible = (isCursorless) ? false : true;

            loadJSON();
            addListeners();

            Instance.gameData = initGameData();

            Instance.gameData.currentScene = currentSceneName;
            Hashtable items = Instance.gameData.items;

            // eventCenter.TriggerChangeScene(SceneType.House01, false);
            eventCenter.TriggerChangeScene(SceneType.House02, false);
            // eventCenter.TriggerChangeScene(SceneType.Cave02a, false);
        }

        private void addListeners()
        {
            eventCenter = EventCenter.Instance;

            eventCenter.OnChangeScene += OnChangeScene;
            eventCenter.OnStartSceneChange += OnStartSceneChange;
            eventCenter.OnCompleteSceneChange += OnCompleteSceneChange;
        }

        private void removeListeners()
        {
            if (eventCenter == null)
            {
                return;
            }
            eventCenter.OnStartSceneChange -= OnStartSceneChange;
            eventCenter.OnCompleteSceneChange -= OnCompleteSceneChange;
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

        public void loadJSON()
        {
            var jsonTextFile = Resources.Load<TextAsset>(JSON_PATH);
            gameJSON = JsonUtility.FromJson<GameJSON>(jsonTextFile.ToString());
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

        private void changeScene(string subScene, int section = -1)
        {
            eventCenter.StartSceneChange(subScene, section);
        }

        private void prepForSceneChange(string subScene, int section = -1)
        {
            Scene currentScene = SceneManager.GetActiveScene();

            if (section > -1)
            {
                Instance.gameData.targetSection = section;
            }

            if (subScene == currentScene.name)
            {
                return;
            }
            wrapUpLevel(currentScene.name, section);
            eventCenter.ContinueSceneChange(subScene, section);
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

        private void loadScene(string subScene)
        {
            cleanUp();
            SceneManager.LoadScene(subScene);
        }


        private void initScene()
        {
            string currentSceneName = currentScene.ToString();
            Debug.Log("Game/onSubSceneLoaded, current subScene = " + currentSceneName);
            SubSceneData subSceneData = getSubSceneDataByName(currentSceneName);
            Debug.Log("  subSceneData.name = " + subSceneData.name + ", isPlayerScene = " + subSceneData.isPlayerScene);
            if (!subSceneData.isPlayerScene)
            {
                return;
            }

            initPlayerScene(subSceneData);
        }

        private void initPlayerScene(SubSceneData subSceneData)
        {
            GameObject levelControllerGO = GameObject.Find(LEVEL_CONTROLLER_GAME_OBJECT);

            if (levelControllerGO == null)
            {
                Debug.LogError("ERROR: Level Controller game object could not be found");
                return;
            }

            levelController = levelControllerGO.GetComponent<LevelController>();
            levelController.Init(Instance.gameData, subSceneData);
        }

        private SubSceneData getSubSceneDataByName(string name)
        {
            foreach (SubSceneData subScene in gameJSON.subScenes)
            {
                if (subScene.name == name)
                {
                    return subScene;
                }
            }
            return new SubSceneData { name = "" };
        }

        private void completeSceneInitialization(bool isLevel, string currentSceneName)
        {
            Instance.isSceneInitialized = true;
            eventCenter.SceneInitializationComplete(currentSceneName);

            InputController inputController = GetComponent<InputController>();
            inputController.Init(isLevel);
        }

        private bool getIsLevel(string subSceneName)
        {
            return LevelUtils.Has(subSceneName, Instance.gameData.levels);
        }

        private void cleanUp()
        {
            levelController = null;
            removeListeners();
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
