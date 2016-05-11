using System;
using UnityEngine;

namespace Polyworks
{
	[System.Serializable]
	public class ItemData
	{
		public string ContainingRoom; 
		public string ItemName;
		public Sprite Icon; 

		public bool IsCollectable = false;
		public bool IsCollected = false;
		public bool IsDroppable = false; 

		public GameObject ItemObject; 

		public ItemData ()
		{
		}

		public ItemData Clone() {
			var clone = new ItemData ();
			clone.ContainingRoom = this.ContainingRoom;
			clone.ItemName = this.ItemName;
			clone.IsCollectable = this.IsCollectable;
			clone.IsCollected = this.IsCollected;
			clone.ItemObject = this.ItemObject;

			return clone;
		}
	}
}

