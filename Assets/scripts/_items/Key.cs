using UnityEngine;
using System.Collections;

public class Key : CollectableItem {

	public LockableArmatureTrigger target;

	public override void Use() {
		EventCenter.Instance.CloseInventoryUI ();
		Inventory.Instance.RemoveItem (this.name, false);
		target.Unlock ();
	}
}
