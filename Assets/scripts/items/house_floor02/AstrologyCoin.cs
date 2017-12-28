using UnityEngine;
using System.Collections;
using Polyworks;

public class AstrologyCoin : Item {
	public string targetName; 

	public override void Use() {
		PiggyBank piggyBank = GameObject.Find (targetName).GetComponent<PiggyBank> ();
		piggyBank.InsertCoin (this.name);
	}
}
