using UnityEngine;
using System.Collections;

namespace Polyworks
{
    public class PlayerManager : MonoBehaviour
    {
        private GameObject playerObject;
        private Player player;
        private Inventory inventory;

        public void OnChangeScene(SceneType type, int targetSection, bool isFadedOut)
        {
            Debug.Log("PlayerManager/OnChangeScene, playerObject = " + playerObject);
            Destroy(playerObject);

            EventCenter eventCenter = EventCenter.Instance;
            if (eventCenter == null)
            {
                return;
            }
            eventCenter.OnChangeScene -= OnChangeScene;
        }

        public void Init(PlayerLocation location, PlayerData data, Hashtable items)
        {
            // Debug.Log("PlayerManager/Init, prefab = " + Game.Instance.playerPrefabName);
            string playerPrefabName = Game.PLAYER_PREFAB_NAME;
            playerObject = (GameObject)Instantiate(Resources.Load(playerPrefabName, typeof(GameObject)), location.position, Quaternion.Euler(location.rotation));
            playerObject.name = playerPrefabName;
            // Debug.Log (" player = " + playerObject);
            GameObject playerGO = playerObject.transform.Find("player").gameObject;
            player = playerGO.GetComponent<Player>();
            inventory = playerGO.GetComponent<Inventory>();

            player.Init(data);
            inventory.Init(items, true);

            EventCenter eventCenter = EventCenter.Instance;
            eventCenter.OnChangeScene += OnChangeScene;

            if (!Game.Instance.GetFlag(Flashlight.COLLECTED))
            {
                return;
            }
            eventCenter.CollectFlashight();
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

