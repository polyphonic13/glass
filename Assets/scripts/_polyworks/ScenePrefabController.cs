using UnityEngine;
using System.Collections;

namespace Polyworks {
	
	public class ScenePrefabController : MonoBehaviour
	{
		public void Init(Prefab[] prefabs, Hashtable items) {
			// Debug.Log ("ScenePrefabController/Init, prefabs.Length = " + prefabs.Length + ", items.Count =  " + items.Count);
			for (int i = 0; i < prefabs.Length; i++) {
				// Debug.Log ("prefabs ["+i+"].name = " + prefabs [i].name);
				bool isAddable = true; 

				if (items.Contains(prefabs [i].name)) {
					// Debug.Log ("gameData.items contains " + prefabs [i].name);
					isAddable = false;
				}

				if(isAddable) {
					// Debug.Log ("isAddable: " + isAddable);
					GameObject go = (GameObject) Instantiate (Resources.Load (prefabs [i].path, typeof(GameObject)), prefabs [i].location, prefabs [i].rotation);
				}
			}
		}

	}
}
