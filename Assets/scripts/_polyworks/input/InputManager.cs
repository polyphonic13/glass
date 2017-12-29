namespace Polyworks {

	using UnityEngine;
	using UnitySampleAssets.CrossPlatformInput;
	using UnitySampleAssets.Utility;
	using Random = UnityEngine.Random;
	using Rewired;
	using Polyworks;
	using System;
	using System.Collections.Generic;

	[Serializable]
	public struct InputButton {
		public string name;
		public bool value;
	}

	[Serializable]
	public struct InputObject {
		public float vertical;
		public float horizontal;
		public InputButton[] inputButtons;
		public Dictionary<string, bool> buttons;
	}

	[Serializable]
	public enum InputContext { PLAYER, MENU, INVENTORY, INSPECTOR, PUZZLE }; 

	public class InputManager : MonoBehaviour {

		public InputObject input; 

		private Rewired.Player _controls;
		private Player _player;
		private CameraZoom _cameraZoom; 

		private MenuUI _menuUI;
		private InventoryUI _inventoryUI;
		private ItemInspector _itemInspector; 
		private PuzzleInspector _puzzleInspector; 

		private bool _isLevel = false;
		private bool _isInitialized = false;

		private bool _isUIOpen = false; 
		private bool _isInventoryOpen = false;
		private bool _isInspectingItem = false; 

		private bool _isInventoryButtonPressed = false;
		private bool _isMenuButtonPressed = false;

		private Item _itemInProximity = null; 

		private EventCenter _eventCenter; 

		private IInputControllable _activeObject = null; 

//		http://answers.unity3d.com/questions/409835/out-of-sync-error-when-iterating-over-a-dictionary.html 
		private List<string> _buttonKeys;

		#region handlers
		public void OnNearItem(Item item, bool isNear) {
//			Debug.Log ("InputManager/OnNearItem, item = " + item.gameObject.name + ", isNear = " + isNear);
			_itemInProximity = (isNear) ? item : null;
		}

		public void OnCloseInventoryUI() {
			_closeInventory ();
		}

		public void OnInspectItem(bool isInspecting, string itemName) {
			_isInspectingItem = isInspecting;
			if (isInspecting) {
				_isUIOpen = true;
			}
		}

		public void OnContextChange(InputContext context, string param) {
//			Debug.Log ("InputManager/OnContextChange, context = " + context);

			if (context == InputContext.PLAYER) {
				if (_player) {
					_player.isActive = true; 
					_activeObject = _player;
				}
			} else {

				_player.isActive = false; 

				if (context == InputContext.PUZZLE) {
//					Debug.Log ("  setting active object to puzzle inspector: " + _puzzleInspector);
					_activeObject = _puzzleInspector;
				}
			}
		}
		#endregion

		public void Init(bool isLevel) {
			_isLevel = isLevel;
			_controls = ReInput.players.GetPlayer(0);

			input.buttons = new Dictionary<string, bool> ();

			for (int i = 0; i < input.inputButtons.Length; i++) {
				input.buttons.Add (input.inputButtons [i].name, false);
			}

			_buttonKeys = new List<string>(input.buttons.Keys);

			GameObject menuObj = GameObject.Find ("menu_ui");
			if (menuObj != null) {
				_menuUI = menuObj.GetComponent<MenuUI> ();
			}

			if (isLevel) {
				GameObject playerObj = GameObject.Find ("player");
				if (playerObj != null) {
					_player = playerObj.GetComponent<Player> ();
					_activeObject = _player;
					_cameraZoom = playerObj.GetComponent<CameraZoom> ();
				}

				GameObject inventoryObj = GameObject.Find ("inventory_ui");
				if (inventoryObj != null) {
					_inventoryUI = inventoryObj.GetComponent<InventoryUI> ();
				}

				_itemInspector = ItemInspector.Instance;

				GameObject puzzleInspector = GameObject.Find ("puzzle_inspector");
				if (puzzleInspector != null) {
					_puzzleInspector = puzzleInspector.GetComponent<PuzzleInspector> ();
				}

				_eventCenter = EventCenter.Instance;
				_eventCenter.OnNearItem += OnNearItem;
				_eventCenter.OnCloseInventoryUI += OnCloseInventoryUI;
				_eventCenter.OnInspectItem += OnInspectItem;
				_eventCenter.OnContextChange += OnContextChange;
			}
			_isInitialized = true;
		}

		#region open / close ui
		private void _openInventory() {
			_openUI ();
			_isInventoryOpen = true;
			_inventoryUI.SetActive (true);
			_activeObject = _inventoryUI;
		}

		private void _openMenu() {
			_openUI ();
			_isInventoryOpen = false;
			_menuUI.SetActive (true);
			_activeObject = _menuUI;
		}

		private void _openUI() {
			_isUIOpen = true;
			_player.isActive = false;
		}

		private void _closeInventory() {
			_isInventoryOpen = false;
			_inventoryUI.SetActive (false);
			_closeUI ();
		}

		private void _closeMenu() {
			_menuUI.SetActive (false);
			_closeUI ();
		}

		private void _closeUI() {
			_isUIOpen = false;
			_player.isActive = true;
			_activeObject = _player;
		}
		#endregion

		#region update loop
		private void _uiUpdate(UIController controller, float horizontal, float vertical) {
			if(controller != null) {
//				controller.SetVertical (vertical);
//				controller.SetHorizontal (horizontal);
				controller.SetConfirm (input.buttons["confirm"]);
				controller.SetCancel (input.buttons["cancel"]);
				controller.SetUp(input.buttons["up"]);
				controller.SetDown(input.buttons["down"]);
				controller.SetLeft(input.buttons["left"]);
				controller.SetRight(input.buttons["right"]);
			}
		}

		private void _inventoryUpdate(float horizontal, float vertical) {
			_uiUpdate (_inventoryUI, horizontal, vertical);
		}

		private void _menuUpdate(float horizontal, float vertical) {
			_uiUpdate (_menuUI, horizontal, vertical);
		}

		private void _itemInspectorUpdate(float horizontal, float vertical) {
//			Debug.Log ("_itemInspectorUpdate, _itemInspector = " + _itemInspector);
			if (_itemInspector != null) {
//				_itemInspector.SetHorizontal (horizontal);
//				_itemInspector.SetVertical (vertical);
				_itemInspector.SetCancel (input.buttons["cancel"]);
				_itemInspector.SetZoomIn (input.buttons["zoom_in"]);
				_itemInspector.SetZoomOut (input.buttons["zoom_out"]);
			}
		}

		private void _playerUpdate(float horizontal, float vertical) {
//			_player.SetHorizontal (horizontal);
//			_player.SetVertical (vertical);

			if(input.buttons["jump"]) {
				_player.SetJumping(true);
			}

			if (_controls.GetButton ("climb")) {
				_player.SetClimbing (true);
			} else if (_controls.GetButtonUp ("climb")) {
				_player.SetClimbing (false);
			}

			_player.SetDiving(input.buttons["dive"]);
			_player.SetCrawling(input.buttons["crawl"]);

			if(_cameraZoom != null && input.buttons["zoom_view"]) {
				_cameraZoom.Execute();
			}
		}

		private void _itemsUpdate() {
			if(_itemInProximity != null && input.buttons["actuate"]) {
//				Debug.Log ("InputManager/_itemsUpdate, calling Actuate on " + _itemInProximity.gameObject.name);
				_itemInProximity.Actuate();
			}

			if(input.buttons["flashlight"]) {
				EventCenter.Instance.EnableFlashlight();
			}
		}

		private void FixedUpdate() {
			if (_isInitialized) {
				input.horizontal = _controls.GetAxis ("move_horizontal");
				input.vertical = _controls.GetAxis ("move_vertical");

				foreach (string key in _buttonKeys) {
					input.buttons [key] = _controls.GetButtonDown(key);
				}

				if (_activeObject != null) {
					_activeObject.SetInput (input);
				}

				if (_activeObject == _puzzleInspector) {
					// let puzzle inspector handle
					_itemsUpdate ();
				} else {
					if (input.buttons["open_menu"]) {
						if (_isInventoryOpen) {
							EventCenter.Instance.CloseInventoryUI ();

							_openMenu ();
						} else if (!_isUIOpen) {
							_openMenu ();
						} else {
							_closeMenu ();
						}
					} else if (input.buttons["open_inventory"]) {
						if (!_isUIOpen) {
							_openInventory ();
						} else {
							if (!_isInventoryOpen) {
								_closeMenu ();
								_openInventory ();
							} else {
								EventCenter.Instance.CloseInventoryUI ();
							}
						}
					} else {
						if (_isInspectingItem) {
							_itemInspectorUpdate (input.horizontal, input.vertical);
						} else if (_isLevel && !_isUIOpen) {
							_playerUpdate (input.horizontal, input.vertical);
							_itemsUpdate ();
						} else if (_isInventoryOpen) {
							_inventoryUpdate (input.horizontal, input.vertical);
						} else {
							_menuUpdate (input.horizontal, input.vertical);
						}
					}
				}
			}
		}
		#endregion

		#region controller maps
		private void _activatePlayerMaps() {
			
		}

		private void _activateMenuMaps() {

		}

		private void _activateMap(string activeMap) {
			_controls.controllers.maps.SetAllMapsEnabled (false);
			_controls.controllers.maps.SetMapsEnabled (true, activeMap);
		}
		#endregion

		private void OnDestroy() {
			EventCenter ec = EventCenter.Instance;
			if (ec != null) {
				_eventCenter.OnNearItem -= OnNearItem;
				_eventCenter.OnCloseInventoryUI -= OnCloseInventoryUI;
				_eventCenter.OnInspectItem -= OnInspectItem;
				_eventCenter.OnContextChange -= OnContextChange;
			}
		}
	}
}