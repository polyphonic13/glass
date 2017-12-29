using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class ItemUtils : MonoBehaviour
	{
		public static bool GetIsUsable(CollectableItemData item, bool isLogOn = false) {
			_log ("item.usableRange = " + item.usableRange, isLogOn);
			if (item.usableRange == null || item.usableRange.distance == 0) {
				_log ("no usableRange, or distance = 0");
				return true;
			}

			UsableRange ur = item.usableRange;
			GameObject targetObject1 = GameObject.Find(ur.target1);
			GameObject targetObject2 = GameObject.Find(ur.target2);

			if(targetObject1 == null || targetObject2 == null) {
				_log("targetObject1 or targetObject2 are null", isLogOn);
				return false;
			}
			_log("ur.target1 = " + ur.target1 
				+ ", ur.target2 = " + ur.target2 
				+ ", targetObject1 = " + targetObject1 
				+ ", targetObject2 = " + targetObject2, isLogOn);

			Transform target1 = targetObject1.transform;
			Transform target2 = targetObject2.transform;

			var distance = Vector3.Distance(target1.position, target2.position);
			_log (" distance = " + distance + ", ur.distance = " + ur.distance, isLogOn);
			if(distance < ur.distance) {
				return true;
			}
			return false;
		}

		private static void _log(string message, bool isLogOn = false) {
			if (isLogOn) {
				Debug.Log (message);
			}
		}


	}
}

