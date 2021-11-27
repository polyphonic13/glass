using UnityEngine;
using System.Collections;

namespace Polyworks
{
    public class PlayerManager : MonoBehaviour
    {
        private Player player;
        private Inventory inventory;

        public void Init(PlayerLocation location, PlayerData data, Hashtable items)
        {
            // Debug.Log("PlayerManager/Init, prefab = " + Game.Instance.playerPrefab);
            string playerPrefab = Game.PLAYER_PREFAB_NAME;
            GameObject playerObject = (GameObject)Instantiate(Resources.Load(playerPrefab, typeof(GameObject)), location.position, Quaternion.Euler(location.rotation));
            playerObject.name = playerPrefab;
            // Debug.Log (" player = " + playerObject);
            GameObject playerGO = playerObject.transform.Find("player").gameObject;
            player = playerGO.GetComponent<Player>();
            inventory = playerGO.GetComponent<Inventory>();

            player.Init(data);
            inventory.Init(items, true);

            if (!Game.Instance.GetFlag(Flashlight.COLLECTED))
            {
                return;
            }
            EventCenter.Instance.CollectFlashight();
        }

        public Player GetPlayer()
        {
            if (player == null)
            {
                return null;
            }
            return player;
        }

        public Inventory GetInventory()
        {
            if (inventory == null)
            {
                return null;
            }
            return inventory;
        }
    }
}

