using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class PlayerManager : MonoBehaviour
	{
		private Player _player;
		private Inventory _inventory;

		public void Init(PlayerLocation location, PlayerData data, Hashtable items) {
			Debug.Log ("PlayerManager/Init, playerPostion = " + location.position + ", rotation = " + location.rotation);
			GameObject playerObject = (GameObject) Instantiate (Resources.Load (Game.Instance.playerPrefab, typeof(GameObject)), location.position, location.rotation);

			GameObject playerGO = playerObject.transform.Find ("player").gameObject;
			_player = playerGO.GetComponent<Player> ();
			_inventory = playerGO.GetComponent<Inventory> ();

			_player.Init (data);
			_inventory.Init (items, true);

			GameObject inventoryObj = GameObject.Find ("inventory_ui");
			if (inventoryObj != null) {
				InventoryUI inventoryUI = inventoryObj.GetComponent<InventoryUI> ();
				// Debug.Log ("_inventoryUI = " + _inventoryUI);
				inventoryUI.InitInventory(_inventory);
			}

		}

		public Player GetPlayer() {
			Debug.Log ("PlayerManager/GetPlayer, _player = " + _player);
			if (_player == null) {
				return null;
			}
			return _player;
		}

		public Inventory GetInventory() {
			if (_inventory == null) {
				return null; 
			}
			return _inventory;
		}
	}
}

