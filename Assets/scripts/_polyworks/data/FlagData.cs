using System;
using UnityEngine;

namespace Polyworks {
	
	public class FlagDataUtils {
		public static int GetIndex(string key, Flag[] list) {
			int index = -1;
			for (var i = 0; i < list.Length; i++) {
				if (list [i].key == key) {
					index = i;
				}
			}
			return index;
		}

		public static Flag GetByIndex(int index, Flag[] list) {
			return list[index];
		}

		public static Flag GetByKey(string key, Flag[] list) {
//			Debug.Log ("FlagDataUtils/GetByKey, key = " + key + ", list = " + list);
			Flag item = new Flag();

			for (var i = 0; i < list.Length; i++) {
//				Debug.Log (" list[" + i + "].key = " + list [i].key);
				if (list [i].key == key) {
					item = list [i];
//					Debug.Log ("  found value = " + item.value);
					break;
				}
			}
			return item;
		}

		public static void SetByKey(string key, bool value, Flag[] list) {
			for (var i = 0; i < list.Length; i++) {
				if (list [i].key == key) {
					list [i].value = value;
					break;
				}
			}
		}
	}

	[Serializable]
	public struct Flag {
		public string key;
		public bool value;
	}
}

