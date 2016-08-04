using UnityEngine;
using System.Collections;
using Polyworks;

public class CrystalKey : CollectableItem {
	public static string EVENT_NAME = "crystalKeyUsed";

	public override void Use() {
		EventCenter.Instance.TriggerStringEvent(EVENT_NAME, this.name);
//		ec.UseCrystalKey (this.name);
	}
}
