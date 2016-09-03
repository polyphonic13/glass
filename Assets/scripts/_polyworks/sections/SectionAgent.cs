using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class SectionAgent : Reaction
	{
		public void ToggleEnabled(bool isEnabled) {
			if (isEnabled) {
//				Debug.Log ("SectionAgent[" + this.name + "]/ToggleEnabled, isEnabled = " + isEnabled);
				this.gameObject.SendMessage ("Enable", null, SendMessageOptions.DontRequireReceiver);
			} else {
				this.gameObject.SendMessage ("Disable", null, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}

