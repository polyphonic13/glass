using System;
using UnityEngine;

namespace Polyworks
{
	[Serializable]
	public class ItemData
	{
		public string containingRoom; 
		public string itemName;
		public string displayName;
		public string prefabName; 

		public bool isCollectable = false;
		public bool isCollected = false;
		public bool isDroppable = false; 
		public bool isUsable = false;

		public ItemData () {}

		public virutal ItemData Clone() {
			var clone = new ItemData ();
			clone.containingRoom = this.containingRoom;
			clone.itemName = this.itemName;
			clone.displayName = this.displayName;
			clone.prefabName = this.prefabName;

			clone.isCollectable = this.isCollectable;
			clone.isCollected = this.isCollected;
			clone.isDroppable = this.isDroppable;
			clone.isUsable = this.isUsable;
			
			return clone;
		}
	}
}

