using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class ItemUtils : MonoBehaviour
	{
		public static bool GetIsUsable(ItemData item) {
			if (item.usableRange == null) {
				Debug.Log ("no usableRange");
				return true;
			}

			UsableRange ur = item.usableRange;
			GameObject targetObject1 = GameObject.Find (ur.target1);
			GameObject targetObject2 = GameObject.Find(ur.target2);

			if(targetObject1 == null || targetObject2 == null) {
				Debug.Log ("targetObject1 or targetObject2 are null");
				return false;
			}

			Transform target1 = targetObject1.transform;
			Transform target2 = targetObject2.transform;

			var distance = Vector3.Distance(target1.position, target2.position);

			if(distance < ur.distance) {
				return true;
			}
			return false;
		}
	}
}

