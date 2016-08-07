using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class SectionAgent : Reaction
	{
		public void ToggleActive(bool isActive) {
			if (isActive) {
				this.gameObject.SendMessage ("Enable");
			} else {
				this.gameObject.SendMessage ("Disable");
			}
		}
	}
}

