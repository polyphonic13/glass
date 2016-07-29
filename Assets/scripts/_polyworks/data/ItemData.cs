using System;
using UnityEngine;

namespace Polyworks
{
	[Serializable]
	public class ItemData
	{
		public string containingSection; 
		public string itemName;
		public string displayName;
		public string prefabName; 

		public bool isCollected = false;
		public bool isDroppable = false; 
		public bool isUsable = false;
		public bool isDestroyedOnUse = false; 

		public int count = 0;

		public string thumbnail; 

		public ItemData () {}

		public virtual ItemData Clone() {
			var clone = new ItemData ();
			clone.containingSection = this.containingSection;
			clone.itemName = this.itemName;
			clone.displayName = this.displayName;
			clone.prefabName = this.prefabName;

			clone.isCollected = this.isCollected;
			clone.isDroppable = this.isDroppable;
			clone.isUsable = this.isUsable;
			clone.isDestroyedOnUse = this.isDestroyedOnUse;

			clone.count = this.count;

			clone.thumbnail = this.thumbnail;

			return clone;
		}
	}
}

