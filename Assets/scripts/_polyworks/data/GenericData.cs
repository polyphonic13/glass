using System;

namespace Polyworks {
	
	public class GenericData {
		public static int GetIndex(string key, GenericItem[] list) {
			int index = -1;
			for (var i = 0; i < list.Length; i++) {
				if (list [i].key == key) {
					index = i;
				}
			}
			return index;
		}

		public static GenericItem GetByIndex(int index, GenericItem[] list) {
			return list[index];
		}

		public static GenericItem GetByKey(string key, GenericItem[] list) {
			GenericItem item = new GenericItem();

			for (var i = 0; i < list.Length; i++) {
				if (list [i].key == key) {
					item = list [i];
				}
			}

			return item;
		}

	}

	[Serializable]
	public struct GenericItem {
		public string key;
		public bool boolVal;
		public int intVal;
		public float floatVal;
		public string stringVal;
	}
}

