using UnityEngine;
using System.Collections;
using Polyworks;

public class AstrologyCoin : Item {

	public PiggyBank piggyBank;

	public override void Use() {
//		Debug.Log ("AstrologyCoin[" + this.name + "]/Use");
		piggyBank.InsertCoin (this.name, this.data.itemName);
		Inventory.Instance.RemoveItem (this.name);
		Destroy (this.gameObject);
	}
}
