using UnityEngine;
using System.Collections;

public class CrystalKey : CollectableItem {

	public override void Use() {
		EventCenter ec = EventCenter.Instance;
		Inventory.Instance.RemoveItem (this.name, false);
		ec.CloseInventoryUI ();
		ec.UseCrystalKey (this.name);
	}
}
