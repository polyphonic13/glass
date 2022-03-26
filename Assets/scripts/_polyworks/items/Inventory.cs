namespace Polyworks
{
    using UnityEngine;
    using System.Collections;

    public class Inventory : MonoBehaviour
    {
        public bool isLogOn = false;

        private Hashtable _items;
        private EventCenter _eventCenter;
        private bool _isPlayerInventory;

        public void Init(Hashtable items = null, bool isPlayerInventory = false)
        {
            Log("Inventory/Init, items.Count = " + items.Count);
            _isPlayerInventory = isPlayerInventory;

            if (items != null && items.Count > 0)
            {
                _items = items;
            }
            else if (_items == null)
            {
                _items = new Hashtable();
            }
        }

        public virtual void AddFromPrefabPath(string path)
        {
            Log("Inventory/AddFromPrefabPath, path = " + path);
            GameObject itemObj = (GameObject)Instantiate(Resources.Load(path, typeof(GameObject)), transform.position, transform.rotation);
            string name = itemObj.name.Replace("(Clone)", "");
            itemObj.name = name;
            Log(" object = " + itemObj);
            if (itemObj != null)
            {
                CollectableItem item = itemObj.GetComponent<CollectableItem>();
                Log(" item = " + item);
                if (item != null)
                {
                    item.Actuate();
                    // item.data.isCollected = true;
                    // Add (item.Clone ());
                    // Destroy (itemObj);
                }
            }

        }

        public virtual void Add(CollectableItemData data, bool increment = true, bool isNoteAdded = true)
        {
            Log("Inventory/Add, item =  " + data.name);
            if (!Contains(data.name))
            {
                _items.Add(data.name, data);
            }
            //			CollectableItemData itemData = Get (data.name) as CollectableItemData;
            if (increment)
            {
                data.count++;
                Log(" incremented count to: " + data.count);
            }
            if (_isPlayerInventory)
            {
                _eventCenter.InventoryAdded(data.name, data.count, _isPlayerInventory);

            }

            if (isNoteAdded)
            {
                _eventCenter.AddNote(data.displayName + " added");
            }
        }

        public virtual CollectableItemData Remove(string name)
        {
            if (!Contains(name))
            {
                return null;
            }
            else
            {
                var data = _items[name] as CollectableItemData;

                if (data != null && data.count > 0)
                {
                    data.count--;

                    if (data.count == 0)
                    {
                        _items.Remove(name);
                    }
                    _eventCenter.InventoryRemoved(data.name, data.count);
                }
                return data;
            }
        }

        public virtual void Use(string name)
        {
            _eventCenter.CloseInventoryUI();
            CollectableItemData data = Get(name);
            Log("Inventory/Use, data = " + data);
            if (data == null)
            {
                return;
            }

            data.isUsable = ItemUtils.GetIsWithinUsableDistance(data, isLogOn);
            Log("  is usable = " + data.isUsable);
            if (!data.isUsable)
            {
                _eventCenter.AddNote("The " + data.displayName + " can not be used here");
                return;
            }

            if (!ItemUtils.GetIsRequiredFlagOn(data, true))
            {
                Log("  flag not on, message = " + data.requiredFlagMessage);
                _eventCenter.AddNote(data.requiredFlagMessage);
                return;
            }

            CollectableItem item = _instantiate(data);

            if (!data.isPersistent)
            {
                Remove(name);
            }

            Log("the item is = " + item);
            item.Use();

            if (item.data.isDestroyedOnUse || item.data.isPersistent)
            {
                Log(" destroying item game object");
                Destroy(item.gameObject);
                return;
            }
            _initDroppedItem(item);
        }

        public virtual void Drop(string name)
        {
            Log("Inventory/Drop, name = " + name);
            CollectableItemData data = Remove(name);
            CollectableItem item = _instantiate(data);

            if (item == null)
            {
                return;
            }

            _initDroppedItem(item);
            _eventCenter.CloseInventoryUI();
        }

        public bool Contains(string key)
        {
            return (_items.Contains(key)) ? true : false;
        }

        public virtual CollectableItemData Get(string name)
        {
            if (Contains(name))
            {
                return _items[name] as CollectableItemData;
            }
            return null;
        }

        public Hashtable GetAll()
        {
            return _items;
        }

        public CollectableItem GetItem(string name)
        {
            return _instantiate(Get(name));
        }

        public void LogAll()
        {
            foreach (string key in _items.Keys)
            {
                Log("key = " + key);
            }
        }

        public void Log(string message)
        {
            if (isLogOn)
            {
                Debug.Log(message);
            }
        }

        private void Awake()
        {
            _eventCenter = EventCenter.Instance;
        }

        private CollectableItem _instantiate(CollectableItemData data)
        {
            if (data == null)
            {
                return null;
            }
            Log("Inventory/_instantiate, prefabPath = " + data.prefabPath);
            GameObject itemObj;

            if (data.prefabPath == "")
            {
                itemObj = new GameObject();
                itemObj.AddComponent<CollectableItem>();
            }
            else
            {
                itemObj = (GameObject)Instantiate(Resources.Load(data.prefabPath, typeof(GameObject)), transform.position, transform.rotation);
            }
            itemObj.name = data.name;
            CollectableItem item = itemObj.GetComponent<CollectableItem>();
            item.data = data;
            item.data.isCollected = item.data.isPersistent;

            return item;
        }

        private void _initDroppedItem(CollectableItem item)
        {
            ProximityAgent pc = item.gameObject.GetComponent<ProximityAgent>();
            if (pc != null)
            {
                pc.Init();
            }

            SectionAgent sa = item.gameObject.GetComponent<SectionAgent>();
            if (sa != null)
            {
                sa.ToggleEnabled(true);
            }
        }

        private void OnDestroy()
        {
            // Log ("Inventory/OnDestroy");
        }

    }
}
