using UnityEngine;
using System.Collections;
using Polyworks;

public class Key : Item {

	public LockableArmatureTrigger target;

	public override void Use() {
		EventCenter.Instance.CloseInventoryUI ();
		Inventory playerInventory = Game.Instance.GetPlayerInventory();
		playerInventory.RemoveItem (this.name, false);
		target.Unlock ();
	}
}
