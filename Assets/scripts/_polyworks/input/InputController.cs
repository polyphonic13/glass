namespace Polyworks
{
    using UnityEngine;
    using Rewired;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public struct InputButton
    {
        public string name;
        public bool value;
    }

    [Serializable]
    public struct InputObject
    {
        public float vertical;
        public float horizontal;
        public InputButton[] inputButtons;
        public Dictionary<string, bool> buttons;
    }

    [Serializable]
    public enum InputContext
    {
        PLAYER,
        MENU,
        INVENTORY,
        INSPECTOR,
        PUZZLE
    };

    public class InputController : MonoBehaviour
    {
        #region members
        public InputObject input;

        // buttons and inputs
        private static readonly string MOVE_HORIZONTAL = "move_horizontal";
        private static readonly string MOVE_VERTICAL = "move_vertical";
        public static readonly string CONFIRM_BUTTON = "confirm";
        public static readonly string CANCEL_BUTTON = "cancel";
        public static readonly string UP_BUTTON = "up";
        public static readonly string DOWN_BUTTON = "down";
        public static readonly string LEFT_BUTTON = "left";
        public static readonly string RIGHT_BUTTON = "right";
        public static readonly string ZOOM_IN_BUTTON = "zoom_in";
        public static readonly string ZOOM_OUT_BUTTON = "zoom_out";
        public static readonly string JUMP_BUTTON = "jump";
        public static readonly string CLIMB_BUTTON = "climp";
        public static readonly string CRAWL_BUTTON = "crawl";
        public static readonly string DIVE_BUTTON = "dive";
        public static readonly string ZOOM_VIEW_BUTTON = "zoom_view";
        public static readonly string ACTUATE_BUTTON = "actuate";
        public static readonly string FLASHLIGHT_BUTTON = "flashlight";
        public static readonly string OPEN_MENU_BUTTON = "open_menu";
        public static readonly string OPEN_INVENTORY_BUTTON = "open_inventory";

        private static readonly string MENU_OBJECT = "menu_ui";
        private static readonly string INVENTORY_OBJECT = "inventory_ui";
        private static readonly string PUZZLE_OBJECT = "puzzle_inspector";
        private static readonly string PLAYER_OBJECT = "player";

        private Rewired.Player controls;
        private Player player;
        private CameraZoom cameraZoom;
        private ItemInspector itemInspector;
        private PuzzleInspector puzzleInspector;
        private Item itemInProximity = null;
        private EventCenter eventCenter;
        private IInputControllable activeInputTarget = null;
        private List<string> buttonKeys;
        private bool isInitialized = false;
        private bool isLevel = false;
        private bool isUIOpen = false;
        private bool isInventoryOpen = false;
        private bool isMenuOpen = false;
        private bool isInspectingItem = false;
        #endregion

        #region handlers
        public void OnSetActiveInputTarget(string type, IInputControllable target)
        {
            Debug.Log("InputController/OnSetActiveInputTarget, target = " + target);
            activeInputTarget = target;
        }

        public void OnNearItem(Item item, bool isNear)
        {
            // Debug.Log ("InputController/OnNearItem, item = " + item.gameObject.name + ", isNear = " + isNear);
            itemInProximity = (isNear) ? item : null;
        }

        public void OnInspectItem(bool isInspecting, string itemName)
        {
            isInspectingItem = isInspecting;
            if (!isInspecting)
            {
                return;
            }
            isUIOpen = true;
        }

        public void OnContextChange(InputContext context, string param)
        {
            // Debug.Log ("InputController/OnContextChange, context = " + context);
            if (context == InputContext.PLAYER)
            {
                if (player)
                {
                    player.isActive = true;
                    activeInputTarget = player;
                }
                return;
            }

            player.isActive = false;

            if (context != InputContext.PUZZLE)
            {
                return;
            }
            // Debug.Log ("  setting active object to puzzle inspector: " + puzzleInspector);
            activeInputTarget = puzzleInspector;
        }

        public void OnCloseMenuUI()
        {
            if (!isMenuOpen)
            {
                return;
            }
            isMenuOpen = false;
            closeUI();
        }

        public void OnCloseInventoryUI()
        {
            if (!isInventoryOpen)
            {
                return;
            }
            isInventoryOpen = false;
            closeUI();
        }
        #endregion

        #region public methods
        public void Init(bool isLevel)
        {
            // Debug.Log("InputController/Init, isLevel = " + isLevel);
            this.isLevel = isLevel;
            controls = ReInput.players.GetPlayer(0);

            input.buttons = new Dictionary<string, bool>();

            for (int i = 0; i < input.inputButtons.Length; i++)
            {
                // Debug.Log("  input button[ " + i + " ].name = " + input.inputButtons[i].name);
                input.buttons.Add(input.inputButtons[i].name, false);
            }

            buttonKeys = new List<string>(input.buttons.Keys);

            isInitialized = true;

            if (!isLevel)
            {
                return;
            }

            initPlayer();
            initPuzzleInspector();
            addLevelEventListeners();
        }
        #endregion

        #region private init
        private void initPlayer()
        {
            GameObject playerObj = GameObject.Find(PLAYER_OBJECT);
            if (playerObj == null)
            {
                return;
            }
            player = playerObj.GetComponent<Player>();
            activeInputTarget = player;
            cameraZoom = playerObj.GetComponent<CameraZoom>();
        }


        private void initPuzzleInspector()
        {
            GameObject puzzleInspectorObj = GameObject.Find(PUZZLE_OBJECT);
            if (puzzleInspectorObj == null)
            {
                return;
            }
            puzzleInspector = puzzleInspectorObj.GetComponent<PuzzleInspector>();
        }

        private void addLevelEventListeners()
        {
            eventCenter = EventCenter.Instance;
            eventCenter.OnSetActiveInputTarget += OnSetActiveInputTarget;
            eventCenter.OnNearItem += OnNearItem;
            eventCenter.OnInspectItem += OnInspectItem;
            eventCenter.OnContextChange += OnContextChange;
            eventCenter.OnCloseMenuUI += OnCloseMenuUI;
            eventCenter.OnCloseInventoryUI += OnCloseInventoryUI;
        }

        private void removeLevelEventListeners()
        {
            if (eventCenter == null)
            {
                return;
            }
            eventCenter.OnSetActiveInputTarget += OnSetActiveInputTarget;
            eventCenter.OnNearItem -= OnNearItem;
            eventCenter.OnInspectItem -= OnInspectItem;
            eventCenter.OnContextChange -= OnContextChange;
            eventCenter.OnCloseMenuUI -= OnCloseMenuUI;
            eventCenter.OnCloseInventoryUI -= OnCloseInventoryUI;
        }
        #endregion

        #region private open / close ui
        private void openInventory()
        {
            Debug.Log("InputController/openInventory, isInventoryOpen = " + isInventoryOpen);
            if (isInventoryOpen || isMenuOpen)
            {
                return;
            }

            eventCenter.OpenInventoryUI();
            isInventoryOpen = true;
            openUI();
        }

        private void openMenu()
        {
            if (isMenuOpen || isInventoryOpen)
            {
                return;
            }

            eventCenter.OpenMenuUI();
            isMenuOpen = true;
            openUI();
        }

        private void openUI()
        {
            isUIOpen = true;
            player.isActive = false;
        }

        private void closeInventory()
        {
            eventCenter.CloseInventoryUI();
        }

        private void closeMenu()
        {
            eventCenter.CloseMenuUI();
        }

        private void closeUI()
        {
            isUIOpen = false;
            player.isActive = true;
            activeInputTarget = player;
        }
        #endregion

        #region private update loop
        private void itemInspectorUpdate(float horizontal, float vertical)
        {
            // Debug.Log("itemInspectorUpdate, itemInspector = " + itemInspector);
            if (itemInspector == null)
            {
                return;
            }

            if (input.buttons[CANCEL_BUTTON])
            {
                itemInspector.SetCancel(true);
                return;
            }

            if (input.buttons[ZOOM_IN_BUTTON])
            {
                itemInspector.SetZoomIn(true);
                return;
            }

            if (input.buttons[ZOOM_OUT_BUTTON])
            {
                itemInspector.SetZoomOut(true);
                return;
            }

            itemInspector.SetHorizontal(horizontal);
            itemInspector.SetVertical(vertical);
        }

        private void playerUpdate(float horizontal, float vertical)
        {
            // player.SetHorizontal (horizontal);
            // player.SetVertical (vertical);

            if (input.buttons[JUMP_BUTTON])
            {
                player.SetJumping(true);
            }

            if (controls.GetButton(CLIMB_BUTTON))
            {
                player.SetClimbing(true);
            }
            else if (controls.GetButtonUp(CLIMB_BUTTON))
            {
                player.SetClimbing(false);
            }

            player.SetDiving(input.buttons[DIVE_BUTTON]);
            player.SetCrawling(input.buttons[CRAWL_BUTTON]);

            if (cameraZoom != null && input.buttons[ZOOM_VIEW_BUTTON])
            {
                cameraZoom.Execute();
            }
        }

        private void itemsUpdate()
        {
            if (itemInProximity != null && input.buttons[ACTUATE_BUTTON])
            {
                // Debug.Log ("InputController/itemsUpdate, calling Actuate on " + itemInProximity.gameObject.name);
                itemInProximity.Actuate();
            }

            if (!input.buttons[FLASHLIGHT_BUTTON])
            {
                return;
            }
            EventCenter.Instance.EnableFlashlight();
        }
        #endregion

        #region private controller maps
        private void activatePlayerMaps()
        {

        }

        private void activateMenuMaps()
        {

        }

        private void activateMap(string activeMap)
        {
            controls.controllers.maps.SetAllMapsEnabled(false);
            controls.controllers.maps.SetMapsEnabled(true, activeMap);
        }
        #endregion

        #region unity methods
        private void FixedUpdate()
        {
            if (!isInitialized)
            {
                return;
            }
            input.horizontal = controls.GetAxis(MOVE_HORIZONTAL);
            input.vertical = controls.GetAxis(MOVE_VERTICAL);

            foreach (string key in buttonKeys)
            {
                input.buttons[key] = controls.GetButtonDown(key);
            }

            if (activeInputTarget != null)
            {
                activeInputTarget.SetInput(input);
            }

            if (activeInputTarget == puzzleInspector as IInputControllable)
            {
                // let puzzle inspector handle input
                itemsUpdate();
                return;
            }

            if (input.buttons[OPEN_MENU_BUTTON])
            {
                openMenu();
                return;
            }

            if (input.buttons[OPEN_INVENTORY_BUTTON])
            {
                openInventory();
                return;
            }

            if (isInspectingItem)
            {
                itemInspectorUpdate(input.horizontal, input.vertical);
                return;
            }

            if (!isLevel || isUIOpen)
            {
                return;
            }
            playerUpdate(input.horizontal, input.vertical);
            itemsUpdate();
        }

        private void OnDestroy()
        {
            if (eventCenter == null)
            {
                return;
            }
            removeLevelEventListeners();
        }
        #endregion
    }
}
