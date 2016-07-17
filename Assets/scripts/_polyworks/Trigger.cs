using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class Trigger : Item
	{
		public string targetName; 

		public override void Use() {
			Triggerable target = GameObject.Find (targetName).GetComponent<Triggerable> ();
			target.Trigger ();
		}
	}

}
