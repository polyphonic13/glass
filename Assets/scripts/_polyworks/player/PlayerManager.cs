using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class PlayerManager : MonoBehaviour
	{
		private Player _player;
		private Inventory _inventory;

		public void Init(PlayerLocation location, PlayerData data, Hashtable items) {
//			Debug.Log ("PlayerManager/Init, prefab = " + Game.Instance.playerPrefab);
			GameObject playerObject = (GameObject) Instantiate (Resources.Load (Game.Instance.playerPrefab, typeof(GameObject)), location.position, Quaternion.Euler(location.rotation));
//			Debug.Log (" player = " + playerObject);
			GameObject playerGO = playerObject.transform.Find ("player").gameObject;
			_player = playerGO.GetComponent<Player> ();
			_inventory = playerGO.GetComponent<Inventory> ();

			_player.Init (data);
			_inventory.Init (items, true);

			if (Game.Instance.GetFlag("isFlashlightCollected")) {
				EventCenter.Instance.CollectFlashight ();
			}
		}

		public Player GetPlayer() {
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

