using UnityEngine;
using System.Collections;
using Polyworks;

public class PortalCrystal : Item {

//	public PortalController portal;
	public string targetName;

	public override void Use() {
		PortalController portal = GameObject.Find (targetName).GetComponent<PortalController> ();
		portal.Activate ();
	}
}
