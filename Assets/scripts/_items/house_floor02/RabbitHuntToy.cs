using UnityEngine;
using System.Collections;
using Polyworks;

public class RabbitHuntToy : Item {
	public override void Use() {
		EventCenter.Instance.InvokeStringEvent (ToyChest.RABBIT_HUNT_ADD_EVENT, this.name);
	}
}
