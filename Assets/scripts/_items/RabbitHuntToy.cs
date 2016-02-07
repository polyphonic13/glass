using UnityEngine;
using System.Collections;

public class RabbitHuntToy : CollectableItem {
	public ToyChest toyChest;

	public override void Use() {
		Debug.Log ("RabbitHuntToy/Use");

		Inventory.Instance.RemoveItem (this.name, false);
		toyChest.AddToy(this);
	}
}
