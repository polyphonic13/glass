using UnityEngine;
using System.Collections;

namespace Polyworks
{

    public class ScenePrefabController : MonoBehaviour
    {
        public static void Init(SectionPrefabs[] sectionsPrefabs, Hashtable items)
        {
            for (int j = 0; j < sectionsPrefabs.Length; j++)
            {
                initPrefabs(sectionsPrefabs[j].prefabs, items);
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
            bool isAddable = true;
            string clonedName = prefab.name + "(Clone)";

            if (items.Contains(clonedName))
            {
                isAddable = false;
            }

            if (isAddable)
            {
                string prefabPath = prefab.path + prefab.name;

                GameObject go = (GameObject)Instantiate(Resources.Load(prefabPath, typeof(GameObject)), prefab.location, Quaternion.Euler(prefab.rotation));
                string addTo = prefab.addTo;
                if (addTo != null && addTo != "")
                {
                    GameObject parentObj = GameObject.Find(addTo);
                    if (parentObj != null)
                    {
                        Transform parentTransform = parentObj.transform;
                        go.transform.parent = parentTransform;
                    }
                }
            }
        }
    }
}
