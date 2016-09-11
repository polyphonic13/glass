using UnityEngine;
using System.Collections;

namespace Polyworks {
	
	public class ScenePrefabController : MonoBehaviour
	{
		public static void Init(SectionPrefabs[] sectionsPrefabs, Hashtable items) {
//			Debug.Log ("ScenePrefabController/Init");
			for (int j = 0; j < sectionsPrefabs.Length; j++) {
				Prefab[] prefabs = sectionsPrefabs[j].prefabs;
				for (int i = 0; i < prefabs.Length; i++) {
					bool isAddable = true; 

					if (items.Contains(prefabs [i].name)) {
						isAddable = false;
					}

					if(isAddable) {
						string prefabPath = prefabs[i].path + prefabs[i].name;
//						Debug.Log ("prefabPath = " + prefabPath);
						GameObject go = (GameObject) Instantiate (Resources.Load (prefabPath, typeof(GameObject)), prefabs [i].location, Quaternion.Euler(prefabs[i].rotation));
						string addTo = prefabs [i].addTo;
						if (addTo != null && addTo != "") {
							GameObject parentObj = GameObject.Find (addTo);
							if (parentObj != null) {
								Transform parentTransform = parentObj.transform;
								go.transform.parent = parentTransform;
							}
						}
					}
				}
			}
			EventCenter.Instance.PrefabsAdded ();
		}
	}
}
