using UnityEngine;
using System.Collections;

namespace Polyworks {
	
	public class ScenePrefabController : MonoBehaviour
	{
		public static void Init(Prefab[] prefabs, Hashtable items) {
			// Debug.Log ("ScenePrefabController/Init, prefabs.Length = " + prefabs.Length + ", items.Count =  " + items.Count);
			for (int i = 0; i < prefabs.Length; i++) {
				// Debug.Log ("prefabs ["+i+"].name = " + prefabs [i].name);
				bool isAddable = true; 

				if (items.Contains(prefabs [i].name)) {
					// Debug.Log ("gameData.items contains " + prefabs [i].name);
					isAddable = false;
				}

				if(isAddable) {
					Debug.Log ("ScenePrefab/Init, prefab = " + prefabs[i].path);
					GameObject go = (GameObject) Instantiate (Resources.Load (prefabs [i].path, typeof(GameObject)), prefabs [i].location, prefabs [i].rotation);
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

	}
}
