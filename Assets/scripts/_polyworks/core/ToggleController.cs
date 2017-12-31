namespace Polyworks {
	using UnityEngine;
	using System.Collections;

	public class ToggleController : Item {

		public Toggler[] _togglers; 
		
		public override void Actuate() {
			Log ("ToggleController[" + this.name + "]/Actuate");
//			base.Actuate ();
			Toggle ();
		}

		public override void Use() {
			Actuate ();
		}

		public void Toggle() {
			for (int i = 0; i < _togglers.Length; i++) {
//				Log  (" _togglers[" + i + "] = " + _togglers [i]);
				if (_togglers [i] != null) {
					_togglers [i].Toggle ();
				}
			}
		}
	}
}