using System;
using UnityEngine;

namespace Polyworks
{
	[Serializable]
	public class ItemData
	{
		public string itemName;
		public string displayName;
		public string prefabPath; 
		public string thumbnail = ""; 

		public bool isCollected = false;
		public bool isDroppable = false; 
		public bool isUsable = false;
		public bool isDestroyedOnUse = false; 

		public int count = 0;

		public UsableRange usableRange; 

		public ItemData () {}

		public virtual ItemData Clone() {
			var clone = new ItemData ();
			clone.itemName = this.itemName;
			clone.displayName = this.displayName;
			clone.prefabPath = this.prefabPath;
			clone.thumbnail = this.thumbnail;

			clone.isCollected = this.isCollected;
			clone.isDroppable = this.isDroppable;
			clone.isUsable = this.isUsable;
			clone.isDestroyedOnUse = this.isDestroyedOnUse;

			clone.count = this.count;

			clone.usableRange = this.usableRange;

			return clone;
		}
	}

	[Serializable]
	public class UsableRange {
		public string target1;
		public string target2;
		public float distance;
	}
}

