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
				inventory.Add (Clone ());
				GameObject.Destroy (gameObject);
			}
		}

		public CollectableItemData Clone() {
			CollectableItemData clone = new CollectableItemData ();
			clone.name = this.name;

			clone.displayName = this.displayName;
			clone.description = this.description;

			clone.prefabPath = this.prefabPath;

			clone.thumbnail = data.thumbnail;

			clone.count = data.count;

			clone.isCollected = data.isCollected;
			clone.isDroppable = data.isDroppable;
			clone.isUsable = data.isUsable;
			clone.isDestroyedOnUse = data.isDestroyedOnUse;
			clone.isPersistent = data.isPersistent; 

			clone.usableRange = data.usableRange;

			return clone;
		}

		public override void Use() {
//			Debug.Log ("CollectableItem[" + this.name + "]/Use");
			Switch[] _switches = gameObject.GetComponents<Switch> ();
			if (_switches != null) {
				for (int i = 0; i < _switches.Length; i++) {
					if (_switches [i] != null) {
						_switches [i].Actuate ();
					}
				}
			}
		}
	}
}
