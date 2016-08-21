using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class CollectableItem : Item
	{
		public override void Actuate(Inventory inventory) {
			// Debug.Log ("CollectableItem[" + this.name + "]/Actuate, isCollected = " + data.isCollected);
			base.Actuate (inventory);

			if (!data.isCollected) {
				EventCenter.Instance.ChangeItemProximity (this, false);
				data.isCollected = true;
				inventory.Add (data.Clone ());
				GameObject.Destroy (gameObject);
			}
		}
	}
}
