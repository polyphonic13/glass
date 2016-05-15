using UnityEngine;
using System.Collections;

namespace Polyworks {
	[System.Serializable]
	public class GameData {
		public string objectName = "serialized_data";
		public string currentTargetScene = "";

		public Hashtable items;
	}
}
