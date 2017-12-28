namespace Polyworks {
	using UnityEngine;
	using System.Collections;

	public class ToggleController : Item {

		public Toggler[] _togglers; 

		public override void Actuate() {
			base.Actuate ();
			Toggle ();
		}

		public override void Use() {
		}

		public void Toggle() {
			for (int i = 0; i < _togglers.Length; i++) {
//				Debug.Log  (" _togglers[" + i + "] = " + _togglers [i]);
				if (_togglers [i] != null) {
					_togglers [i].Toggle ();
				}
			}
		}
	}
}