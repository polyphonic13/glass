using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class SectionAgent : Reaction
	{
		public void ToggleEnabled(bool isEnabled) {
//			Debug.Log ("SectionAgent[" + this.name + "]/ToggleEnabled, isEnabled = " + isEnabled);
			if (isEnabled) {
				this.gameObject.SendMessage ("Enable");
			} else {
				this.gameObject.SendMessage ("Disable");
			}
		}
	}
}

