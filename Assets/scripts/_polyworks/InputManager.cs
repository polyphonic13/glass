using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;
using UnitySampleAssets.Utility;
using Random = UnityEngine.Random;
using Rewired;
using Polyworks;

namespace Polyworks {
	
	public class InputManager : MonoBehaviour {
	
		private Rewired.Player _controls;
		private Player _player;
	
		private bool _isUIOpen = false; 
		private bool _isMenuOpen = false; 
		private bool _isInventoryOpen = false;
		
		private Item _itemInProximity = null; 
		
		public void OnNearItem(Item item, bool isNear) {
			_itemInProximity = (isNear) ? item : null;
		}
		
		private void Awake() {
			_controls = ReInput.players.GetPlayer(0);
			_player = GameObject.GetComponent<Player>();
			
			EventCenter ec = EventCenter.Instance;
			ec.OnNearItem += this.OnNearItem;
		}
		
		private void Update() {
			if(_isUIOpen) {
				if(_controls.GetButton("cancel")) {
					_isUIOpen = false;
					// close ui
				}
			} else {
				if(_controls.GetButton("open_menu")) {
					_isUIOpen = true;
					_isMenuOpen = true;
					// open menu
				} else if(_controls.GetButton("open_inventory")) {
					_isUIOpen = true;
					_isInventoryOpen = true;
					// open inventory
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
		}
		
	}
}