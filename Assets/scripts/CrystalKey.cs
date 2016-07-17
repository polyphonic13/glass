using UnityEngine;
using System.Collections;
using Polyworks;

public class CrystalKey : CollectableItem {

	public override void Use() {
		EventCenter ec = EventCenter.Instance;
		ec.UseCrystalKey (this.name);
	}
}
