using UnityEngine;
using System.Collections;

public class PortalCrystal : CollectableItem {

	public PortalController portal;

	public override void Use() {
		EventCenter.Instance.CloseInventoryUI ();
		Inventory.Instance.RemoveItem (this.name, false);
		portal.Activate ();
	}
}
