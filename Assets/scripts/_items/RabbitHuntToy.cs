using UnityEngine;
using System.Collections;
using Polyworks;

public class RabbitHuntToy : Item {
	public string targetName = "toychest"; 

	public override void Use() {
		ToyChest toyChest = GameObject.Find (targetName).GetComponent<ToyChest> ();
		toyChest.AddToy(this);
	}
}
