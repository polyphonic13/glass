using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class CollectableItem : Item
	{
		public CollectableItemData data;

		public override void Actuate() {
			// Debug.Log ("CollectableItem[" + this.name + "]/Actuate, isCollected = " + data.isCollected);

			if (!data.isCollected) {
				Inventory inventory = Game.Instance.GetPlayerInventory ();
				EventCenter.Instance.ChangeItemProximity (this, false);
				data.isCollected = true;
				inventory.Add (data.Clone ());
				GameObject.Destroy (gameObject);
			}
		}
		
		public override void Use() {
			SendMessage("Use", null, SendMessageOptions.DontRequireReceiver);
		}
	}
}
