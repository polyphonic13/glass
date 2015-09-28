using UnityEngine;
using System.Collections;

public class RabbitHuntToy : CollectableItem {
	public ToyChest toyChest;

	public override void Use() {
		Inventory.Instance.RemoveItem (this.name, false);
		toyChest.AddToy(this);
	}
}
