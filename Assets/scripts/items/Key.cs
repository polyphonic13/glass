using UnityEngine;
using System.Collections;
using Polyworks;

public class Key : Item {

	public string targetName; 

	public override void Use() {
		LockableArmatureTrigger target = GameObject.Find (targetName).GetComponent<LockableArmatureTrigger> ();
		target.Unlock ();
	}
}
