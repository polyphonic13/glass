using UnityEngine;
using System.Collections;

public class GameObjectUtils : MonoBehaviour
{
	public static void DeactivateFromTransforms(Transform[] transforms) {
		for (int i = 0; i < transforms.Length; i++) {
			if (transforms [i].gameObject != null) {
				transforms [i].gameObject.SetActive (false);
			}
		}
	}

}

