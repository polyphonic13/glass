using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;
using UnitySampleAssets.Utility;
using Random = UnityEngine.Random;
using Rewired;
using Polyworks;

namespace Polyworks {
	
	public class InputManager : MonoBehaviour {
		public bool isPlayerActive; 

		private Rewired.Player _controls;
		private Player _player;
	
		private MenuUI _menuUI;
		private InventoryUI _inventoryUI;

		private bool _isUIOpen = false; 

		private Item _itemInProximity = null; 
		
		public void OnNearItem(Item item, bool isNear) {
			_itemInProximity = (isNear) ? item : null;
		}
		
		private void Awake() {
			_controls = ReInput.players.GetPlayer(0);

			GameObject playerObj = GameObject.Find ("player");
			if (playerObj != null) {
				_player = playerObj.GetComponent<Player> ();
			}

			GameObject menuObj = GameObject.Find ("menu_ui");
			if (menuObj != null) {
				_menuUI.GetComponent<MenuUI> ();
			}

			GameObject inventoryObj = GameObject.Find ("inventory_ui");
			if (inventoryObj != null) {
				_inventoryUI = inventoryObj.GetComponent<InventoryUI> ();
			}

			EventCenter ec = EventCenter.Instance;
			ec.OnNearItem += this.OnNearItem;
		}
		
		private void Update() {
			if(_isUIOpen) {
				if(_controls.GetButton("cancel")) {
					_isUIOpen = false;
					_player.isActive = true;
					_inventoryUI.SetActive(false);
					_menuUI.SetActive (false);
				}
			} else {
				if(_controls.GetButton("open_menu")) {
					_isUIOpen = true;
					_player.isActive = false;
					_inventoryUI.SetActive(false);
					_menuUI.SetActive (true);
				} else if(_controls.GetButton("open_inventory")) {
					_isUIOpen = true;
					_player.isActive = false;
					_inventoryUI.SetActive(true);
					_menuUI.SetActive (false);
				} else {
					
					if(_itemInProximity != null && _controls.GetButton("actuate")) {
						_itemInProximity.Actuate();
					}

					if(_controls.GetButton("jump")) {
						_player.SetJumping(true);
					}

					_player.SetClimbing(_controls.GetButton("climb"));
					_player.SetDiving(_controls.GetButton("dive"));
					_player.SetCrawling(_controls.GetButton("crawl"));

					if(_controls.GetButton("flashlight")) {
						EventCenter.Instance.ActuateFlashlight();
					}
				}
			}
		}

		private void FixedUpdate() {
			float horizontal = _controls.GetAxis("move_horizontal");
			float vertical = _controls.GetAxis("move_vertical");
			if (!_isUIOpen) {
				_player.SetHorizontal (horizontal);
				_player.SetVertical (vertical);
			} else if (_inventoryCanvas.enabled) {

			}
		}

		private void _activatePlayerMaps() {
			
		}

		private void _activateMenuMaps() {

		}

		private void _activateMap(string activeMap) {
			_controls.controllers.maps.SetAllMapsEnabled (false);
			_controls.controllers.maps.SetMapsEnabled (true, activeMap);
		}
	}
}