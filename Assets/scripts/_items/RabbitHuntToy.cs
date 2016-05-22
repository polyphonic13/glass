using UnityEngine;
using System.Collections;
using Polyworks;

public class RabbitHuntToy : Item {
	public ToyChest toyChest;

	public override void Use() {
		Debug.Log ("RabbitHuntToy/Use");

		Inventory.Instance.RemoveItem (this.name, false);
		toyChest.AddToy(this);
	}
}
