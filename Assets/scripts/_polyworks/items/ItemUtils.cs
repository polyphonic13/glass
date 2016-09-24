using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class ItemUtils : MonoBehaviour
	{
		public static bool GetIsUsable(CollectableItemData item) {
			Debug.Log ("item.usableRange = " + item.usableRange);
			if (item.usableRange == null || item.usableRange.distance == 0) {
				Debug.Log ("no usableRange, or distance = 0");
				return true;
			}

			UsableRange ur = item.usableRange;
			GameObject targetObject1 = GameObject.Find(ur.target1);
			GameObject targetObject2 = GameObject.Find(ur.target2);

			if(targetObject1 == null || targetObject2 == null) {
				Debug.Log ("targetObject1 or targetObject2 are null");
				return false;
			}
			Debug.Log ("ur.target1 = " + ur.target1 + ", ur.target2 = " + ur.target2 + ", targetObject1 = " + targetObject1 + ", targetObject2 = " + targetObject2);

			Transform target1 = targetObject1.transform;
			Transform target2 = targetObject2.transform;

			var distance = Vector3.Distance(target1.position, target2.position);
			Debug.Log (" distance = " + distance + ", ur.distance = " + ur.distance);
			if(distance < ur.distance) {
				return true;
			}
			return false;
		}
	}
}

