namespace Polyworks {

	using UnityEngine;
	using UnitySampleAssets.CrossPlatformInput;
	using UnitySampleAssets.Utility;
	using Random = UnityEngine.Random;
	using Rewired;
	using Polyworks;

	public class InputManager : MonoBehaviour {
		private Rewired.Player _controls;
		private Player _player;
		private CameraZoom _cameraZoom; 

		private MenuUI _menuUI;
		private InventoryUI _inventoryUI;
		private ItemInspector _itemInspector; 

		private bool _isLevel = false;
		private bool _isInitialized = false;

		private bool _isUIOpen = false; 
		private bool _isInventoryOpen = false;
		private bool _isInspectingItem = false; 

		private bool _isInventoryButtonPressed = false;
		private bool _isMenuButtonPressed = false;

		private Item _itemInProximity = null; 

		private IInputControllable _activeObject = null; 

		#region handlers
		public void OnNearItem(Item item, bool isNear) {
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

		public void OnContextChange(string type) {
			if (type == "player") {
				if (_player) {
					_player.isActive = true; 
					_activeObject = _player;
				}
			} else {
				_player.isActive = false; 

			}
		}
		#endregion

		public void Init(bool isLevel) {
//			Debug.Log("InputManager/Init");
			_isLevel = isLevel;
			_controls = ReInput.players.GetPlayer(0);

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

				EventCenter ec = EventCenter.Instance;
				ec.OnNearItem += this.OnNearItem;
				ec.OnCloseInventoryUI += this.OnCloseInventoryUI;
				ec.OnInspectItem += this.OnInspectItem;
				ec.OnContextChange += this.OnContextChange;
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
				controller.SetConfirm (_controls.GetButtonDown ("confirm"));
				controller.SetCancel (_controls.GetButtonDown ("cancel"));
				controller.SetUp(_controls.GetButtonDown("up"));
				controller.SetDown(_controls.GetButtonDown("down"));
				controller.SetLeft(_controls.GetButtonDown("left"));
				controller.SetRight(_controls.GetButtonDown("right"));
			}
		}

		private void _inventoryUpdate(float horizontal, float vertical) {
			_uiUpdate (_inventoryUI, horizontal, vertical);
		}

		private void _menuUpdate(float horizontal, float vertical) {
			_uiUpdate (_menuUI, horizontal, vertical);
		}

		private void _itemInspectorUpdate(float horizontal, float vertical) {
			Debug.Log ("_itemInspectorUpdate, _itemInspector = " + _itemInspector);
			if (_itemInspector != null) {
//				_itemInspector.SetHorizontal (horizontal);
//				_itemInspector.SetVertical (vertical);
				_itemInspector.SetCancel (_controls.GetButtonDown ("cancel"));
				_itemInspector.SetZoomIn (_controls.GetButtonDown ("zoom_in"));
				_itemInspector.SetZoomOut (_controls.GetButtonDown ("zoom_out"));
			}
		}

		private void _playerUpdate(float horizontal, float vertical) {
//			_player.SetHorizontal (horizontal);
//			_player.SetVertical (vertical);

			if(_controls.GetButtonDown("jump")) {
				_player.SetJumping(true);
			}

			if (_controls.GetButton ("climb")) {
				_player.SetClimbing (true);
			} else if (_controls.GetButtonUp ("climb")) {
				_player.SetClimbing (false);
			}

			_player.SetDiving(_controls.GetButtonDown("dive"));
			_player.SetCrawling(_controls.GetButtonDown("crawl"));

			if(_cameraZoom != null && _controls.GetButtonDown("zoom_view")) {
				_cameraZoom.Execute();
			}
		}

		private void _itemsUpdate() {
			if(_itemInProximity != null && _controls.GetButtonDown("actuate")) {
				_itemInProximity.Actuate();
			}

			if(_controls.GetButtonDown("flashlight")) {
				EventCenter.Instance.EnableFlashlight();
			}
		}

		private void FixedUpdate() {
			if (_isInitialized) {
				float horizontal = _controls.GetAxis ("move_horizontal");
				float vertical = _controls.GetAxis ("move_vertical");
				_isMenuButtonPressed = _controls.GetButtonDown ("open_menu");
				_isInventoryButtonPressed = _controls.GetButtonDown ("open_inventory");

				if (_activeObject != null) {
					_activeObject.SetVertical (vertical);
					_activeObject.SetHorizontal (horizontal);
				}

				if (_isMenuButtonPressed) {
					if (_isInventoryOpen) {
						EventCenter.Instance.CloseInventoryUI ();

						_openMenu ();
					} else if (!_isUIOpen) {
						_openMenu ();
					} else {
						_closeMenu ();
					}
				} else if (_isInventoryButtonPressed) {
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
						_itemInspectorUpdate (horizontal, vertical);
					} else if (_isLevel && !_isUIOpen) {
						_playerUpdate (horizontal, vertical);
						_itemsUpdate ();
					} else if (_isInventoryOpen) {
						_inventoryUpdate (horizontal, vertical);
					} else {
						_menuUpdate (horizontal, vertical);
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
				ec.OnNearItem -= this.OnNearItem;
				ec.OnCloseInventoryUI -= this.OnCloseInventoryUI;
				ec.OnInspectItem -= this.OnInspectItem;
			}
		}
	}
}