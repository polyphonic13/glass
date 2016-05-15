using UnityEngine;
using System.Collections;

public class AstrologyCoin : CollectableItem {

	public PiggyBank piggyBank;

	public override void Use() {
//		Debug.Log ("AstrologyCoin[" + this.name + "]/Use");
		piggyBank.InsertCoin (this.name, this.itemName);
		Inventory.Instance.RemoveItem (this.name);
		Destroy (this.gameObject);
	}
}
