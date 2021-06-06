namespace Polyworks
{
    using UnityEngine;
    using System.Collections;

    public class ScenePrefabController : MonoBehaviour
    {
        public static void Init(Section[] sections, Hashtable items)
        {
            foreach (Section section in sections)
            {
                initPrefabs(section.prefabs, items);
            }

            EventCenter.Instance.PrefabsAdded();
        }

        private static void initPrefabs(PrefabData[] prefabs, Hashtable items)
        {
            foreach (PrefabData prefab in prefabs)
            {
                initPrefab(prefab, items);
            }
        }

        private static void initPrefab(PrefabData prefab, Hashtable items)
        {
            string clonedName = prefab.name;
            bool isAddable = !(items.Contains(clonedName));

            if (!isAddable)
            {
                return;
            }
            string prefabPath = prefab.path + prefab.name;

            Vector3 position = new Vector3(prefab.position.x, prefab.position.y, prefab.position.z);
            Vector3 rotation = new Vector3(prefab.rotation.x, prefab.rotation.y, prefab.rotation.z);
            // Debug.Log("prefabPath = " + prefabPath);
            GameObject go = (GameObject)Instantiate(Resources.Load(prefabPath, typeof(GameObject)), position, Quaternion.Euler(rotation));
            go.name = prefab.name;

            string addTo = prefab.addTo;

            if (addTo == null || addTo == "")
            {
                return;
            }

            GameObject parentObj = GameObject.Find(addTo);
            if (parentObj == null)
            {
                return;
            }
            go.transform.SetParent(parentObj.transform);
        }
    }
}
