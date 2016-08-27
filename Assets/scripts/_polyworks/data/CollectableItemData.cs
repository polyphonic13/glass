using System;

namespace Polyworks 
{
	[Serializable]
	public class CollectableItemData: ItemData 
	{
		public bool isCollected = false;
		public bool isDroppable = false; 
		public bool isUsable = false;
		public bool isDestroyedOnUse = false; 

		public int count = 0;

		public UsableRange usableRange; 

		public CollectableItemData () {}

		public CollectableItemData Clone() {
			var clone = new CollectableItemData();
			clone.itemName = this.itemName;
			clone.displayName = this.displayName;
			clone.prefabPath = this.prefabPath;
			clone.thumbnail = this.thumbnail;

			clone.isCollected = this.isCollected;
			clone.isDroppable = this.isDroppable;
			clone.isUsable = this.isUsable;
			clone.isDestroyedOnUse = this.isDestroyedOnUse;

			clone.usableRange = this.usableRange;
			return clone;
		}
	}
}

