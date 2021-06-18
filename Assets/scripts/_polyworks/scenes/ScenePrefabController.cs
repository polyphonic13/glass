using UnityEngine;
using System.Collections;

namespace Polyworks
{

    public class ScenePrefabController : MonoBehaviour
    {
        public static void Init(SectionPrefabs[] sectionPrefabs, Hashtable items)
        {
            foreach (SectionPrefabs sectionPrefab in sectionPrefabs)
            {
                initPrefabs(sectionPrefab.prefabs, items);
            }

            EventCenter.Instance.PrefabsAdded();
        }

        private static void initPrefabs(Prefab[] prefabs, Hashtable items)
        {
            for (int i = 0; i < prefabs.Length; i++)
            {
                initPrefab(prefabs[i], items);
            }
        }

        private static void initPrefab(Prefab prefab, Hashtable items)
        {
            string clonedName = prefab.name;
            bool isAddable = !(items.Contains(clonedName));

            if (!isAddable)
            {
                return;
            }
            string prefabPath = prefab.path + prefab.name;

            GameObject go = (GameObject)Instantiate(Resources.Load(prefabPath, typeof(GameObject)), prefab.location, Quaternion.Euler(prefab.rotation));
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
