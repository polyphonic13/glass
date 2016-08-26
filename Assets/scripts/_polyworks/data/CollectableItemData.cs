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

		public CollectableItemData () {}

		public override IData Clone() {
			var clone = base.Clone();

			clone.isCollected = this.isCollected;
			clone.isDroppable = this.isDroppable;
			clone.isUsable = this.isUsable;
			clone.isDestroyedOnUse = this.isDestroyedOnUse;
		}
	}
}

