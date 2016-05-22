using UnityEngine;
using System.Collections;
using Polyworks;

public class PortalCrystal : Item {

	public PortalController portal;

	public override void Use() {
		EventCenter.Instance.CloseInventoryUI ();
		Inventory.Instance.RemoveItem (this.name, false);
		portal.Activate ();
	}
}
