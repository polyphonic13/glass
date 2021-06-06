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
    public enum InputContext { PLAYER, MENU, INVENTORY, INSPECTOR, PUZZLE };

    public class InputManager : MonoBehaviour
    {
        public InputObject input;

        private static readonly string MENU_OBJECT = "menu_ui";
        private static readonly string PLAYER_OBJECT = "player";
        private static readonly string INVENTORY_OBJECT = "inventory_ui";
        private static readonly string PUZZLE_OBJECT = "puzzle_inspector";
        // buttons and inputs
        private static readonly string MOVE_HORIZONTAL = "move_horizontal";
        private static readonly string MOVE_VERTICAL = "move_vertical";
        private static readonly string CONFIRM_BUTTON = "confirm";
        private static readonly string CANCEL_BUTTON = "cancel";
        private static readonly string UP_BUTTON = "up";
        private static readonly string DOWN_BUTTON = "down";
        private static readonly string LEFT_BUTTON = "left";
        private static readonly string RIGHT_BUTTON = "right";
        private static readonly string ZOOM_IN_BUTTON = "zoom_in";
        private static readonly string ZOOM_OUT_BUTTON = "zoom_out";
        private static readonly string JUMP_BUTTON = "jump";
        private static readonly string CLIMB_BUTTON = "climp";
        private static readonly string CRAWL_BUTTON = "crawl";
        private static readonly string DIVE_BUTTON = "dive";
        private static readonly string ZOOM_VIEW_BUTTON = "zoom_view";
        private static readonly string ACTUATE_BUTTON = "actuate";
        private static readonly string FLASH_LIGHT_BUTTON = "flashlight";
        private static readonly string OPEN_MENU_BUTTON = "open_menu";
        private static readonly string OPEN_INVENTORY_BUTTON = "open_inventory";

        private Rewired.Player controls;
        private Player player;
        private CameraZoom cameraZoom;

        private MenuUI menuUI;
        private InventoryUI inventoryUI;
        private ItemInspector itemInspector;
        private PuzzleInspector puzzleInspector;

        private bool isInitialized = false;
        private bool isLevel = false;

        private bool isUIOpen = false;
        private bool isInventoryOpen = false;
        private bool isInspectingItem = false;

        private bool isInventoryButtonPressed = false;
        private bool isMenuButtonPressed = false;

        private Item itemInProximity = null;

        private EventCenter eventCenter;

        private IInputControllable activeObject = null;

        // http://answers.unity3d.com/questions/409835/out-of-sync-error-when-iterating-over-a-dictionary.html
        private List<string> buttonKeys;

        #region handlers
        public void OnNearItem(Item item, bool isNear)
        {
            // Debug.Log ("InputManager/OnNearItem, item = " + item.gameObject.name + ", isNear = " + isNear);
            itemInProximity = (isNear) ? item : null;
        }

        public void OnCloseInventoryUI()
        {
            closeInventory();
        }

        public void OnInspectItem(bool isInspecting, string itemName)
        {
            isInspectingItem = isInspecting;
            if (isInspecting)
            {
                isUIOpen = true;
            }
        }

        public void OnContextChange(InputContext context, string param)
        {
            // Debug.Log ("InputManager/OnContextChange, context = " + context);

            if (context == InputContext.PLAYER)
            {
                if (player)
                {
                    player.isActive = true;
                    activeObject = player;
                }
            }
            else
            {

                player.isActive = false;

                if (context == InputContext.PUZZLE)
                {
                    // Debug.Log ("  setting active object to puzzle inspector: " + puzzleInspector);
                    activeObject = puzzleInspector;
                }
            }
        }
        #endregion

        public void Init(bool isLevel)
        {
            this.isLevel = isLevel;
            controls = ReInput.players.GetPlayer(0);

            input.buttons = new Dictionary<string, bool>();

            for (int i = 0; i < input.inputButtons.Length; i++)
            {
                input.buttons.Add(input.inputButtons[i].name, false);
            }

            buttonKeys = new List<string>(input.buttons.Keys);

            GameObject menuObj = GameObject.Find(MENU_OBJECT);
            if (menuObj != null)
            {
                menuUI = menuObj.GetComponent<MenuUI>();
            }

            if (isLevel)
            {
                GameObject playerObj = GameObject.Find(PLAYER_OBJECT);
                if (playerObj != null)
                {
                    player = playerObj.GetComponent<Player>();
                    activeObject = player;
                    cameraZoom = playerObj.GetComponent<CameraZoom>();
                }

                GameObject inventoryObj = GameObject.Find(INVENTORY_OBJECT);
                if (inventoryObj != null)
                {
                    inventoryUI = inventoryObj.GetComponent<InventoryUI>();
                }

                itemInspector = ItemInspector.Instance;

                GameObject puzzleInspectorObj = GameObject.Find(PUZZLE_OBJECT);
                if (puzzleInspectorObj != null)
                {
                    puzzleInspector = puzzleInspectorObj.GetComponent<PuzzleInspector>();
                }

                eventCenter = EventCenter.Instance;
                eventCenter.OnNearItem += OnNearItem;
                eventCenter.OnCloseInventoryUI += OnCloseInventoryUI;
                eventCenter.OnInspectItem += OnInspectItem;
                eventCenter.OnContextChange += OnContextChange;
            }
            isInitialized = true;
        }

        #region open / close ui
        private void openInventory()
        {
            openUI();
            isInventoryOpen = true;
            inventoryUI.SetActive(true);
            activeObject = inventoryUI;
        }

        private void openMenu()
        {
            openUI();
            isInventoryOpen = false;
            menuUI.SetActive(true);
            activeObject = menuUI;
        }

        private void openUI()
        {
            isUIOpen = true;
            player.isActive = false;
        }

        private void closeInventory()
        {
            isInventoryOpen = false;
            inventoryUI.SetActive(false);
            closeUI();
        }

        private void closeMenu()
        {
            menuUI.SetActive(false);
            closeUI();
        }

        private void closeUI()
        {
            isUIOpen = false;
            player.isActive = true;
            activeObject = player;
        }
        #endregion

        #region update loop
        private void uiUpdate(UIController controller, float horizontal, float vertical)
        {
            if (controller == null)
            {
                return;
            }
            // controller.SetVertical (vertical);
            // controller.SetHorizontal (horizontal);
            controller.SetConfirm(input.buttons[CONFIRM_BUTTON]);
            controller.SetCancel(input.buttons[CANCEL_BUTTON]);
            controller.SetUp(input.buttons[UP_BUTTON]);
            controller.SetDown(input.buttons[DOWN_BUTTON]);
            controller.SetLeft(input.buttons[LEFT_BUTTON]);
            controller.SetRight(input.buttons[RIGHT_BUTTON]);
        }

        private void inventoryUpdate(float horizontal, float vertical)
        {
            uiUpdate(inventoryUI, horizontal, vertical);
        }

        private void menuUpdate(float horizontal, float vertical)
        {
            uiUpdate(menuUI, horizontal, vertical);
        }

        private void itemInspectorUpdate(float horizontal, float vertical)
        {
            // Debug.Log ("itemInspectorUpdate, itemInspector = " + itemInspector);
            if (itemInspector == null)
            {
                return;
            }
            // itemInspector.SetHorizontal (horizontal);
            // itemInspector.SetVertical (vertical);
            itemInspector.SetCancel(input.buttons[CANCEL_BUTTON]);
            itemInspector.SetZoomIn(input.buttons[ZOOM_IN_BUTTON]);
            itemInspector.SetZoomOut(input.buttons[ZOOM_OUT_BUTTON]);
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
                // Debug.Log ("InputManager/itemsUpdate, calling Actuate on " + itemInProximity.gameObject.name);
                itemInProximity.Actuate();
            }

            // if(input.buttons[FLASHLIGHT_BUTTON]) {
            // 	EventCenter.Instance.EnableFlashlight();
            // }
        }

        private void handleOpenMenuInput()
        {
            if (isInventoryOpen)
            {
                EventCenter.Instance.CloseInventoryUI();
                openMenu();
                return;
            }
            if (!isUIOpen)
            {
                openMenu();
                return;
            }
            closeMenu();
        }

        private void handleOpenInventoryInput()
        {
            if (!isUIOpen)
            {
                openInventory();
                return;
            }
            if (!isInventoryOpen)
            {
                closeMenu();
                openInventory();
                return;
            }
            EventCenter.Instance.CloseInventoryUI();
        }
        #endregion

        #region controller maps
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

            if (activeObject != null)
            {
                activeObject.SetInput(input);
            }

            if (activeObject == puzzleInspector as IInputControllable)
            {
                // let puzzle inspector handle input
                itemsUpdate();
                return;
            }

            if (input.buttons[OPEN_MENU_BUTTON])
            {
                handleOpenMenuInput();
                return;
            }

            if (input.buttons[OPEN_INVENTORY_BUTTON])
            {
                handleOpenInventoryInput();
                return;
            }

            if (isInspectingItem)
            {
                itemInspectorUpdate(input.horizontal, input.vertical);
                return;
            }

            if (isLevel && !isUIOpen)
            {
                playerUpdate(input.horizontal, input.vertical);
                itemsUpdate();
                return;
            }

            if (isInventoryOpen)
            {
                inventoryUpdate(input.horizontal, input.vertical);
                return;
            }

            menuUpdate(input.horizontal, input.vertical);
        }

        private void OnDestroy()
        {
            if (eventCenter != null)
            {
                eventCenter.OnNearItem -= OnNearItem;
                eventCenter.OnCloseInventoryUI -= OnCloseInventoryUI;
                eventCenter.OnInspectItem -= OnInspectItem;
                eventCenter.OnContextChange -= OnContextChange;
            }
        }
        #endregion
    }
}
