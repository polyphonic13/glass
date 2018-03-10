namespace Polyworks {
	using UnityEngine;
	using System.Collections;

	public class FlagToggler : Toggler {
		public string key;

		public override void Toggle() {
			Log ("FlagToggler[" + this.name + "]/Toggle, key = " + key);
			base.Toggle ();
			Game.Instance.SetFlag (key, isOn);
		}
	}
}
