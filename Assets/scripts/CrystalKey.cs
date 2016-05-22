using UnityEngine;
using System.Collections;
using Polyworks;

public class CrystalKey : CollectableItem {

	public override void Use() {
		EventCenter ec = EventCenter.Instance;
		Inventory.Instance.RemoveItem (this.name, false);
		ec.CloseInventoryUI ();
		Debug.Log ("CrystalKey[" + this.name + "]/Use");
		ec.UseCrystalKey (this.name);
	}
}
