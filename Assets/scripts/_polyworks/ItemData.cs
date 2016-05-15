using System;
using UnityEngine;

namespace Polyworks
{
	[Serializable]
	public class ItemData
	{
		public string containingRoom; 
		public string itemName;
		public Sprite Icon; 

		public bool isCollectable = false;
		public bool isCollected = false;
		public bool isDroppable = false; 

		public GameObject itemObject; 

		public ItemData ()
		{
		}

		public ItemData Clone() {
			var clone = new ItemData ();
			clone.containingRoom = this.containingRoom;
			clone.itemName = this.itemName;
			clone.isCollectable = this.isCollectable;
			clone.isCollected = this.isCollected;
			clone.itemObject = this.itemObject;

			return clone;
		}
	}
}

