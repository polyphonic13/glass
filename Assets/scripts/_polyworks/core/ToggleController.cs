namespace Polyworks {
	using UnityEngine;
	using System.Collections;

	public class ToggleController : ActuateAgent {

		public Toggler[] _togglers; 

		public override void Actuate() {
			Toggle ();
		}

		public override void Use() {
		}

		public void Toggle() {
			for (int i = 0; i < _togglers.Length; i++) {
				_togglers [i].Toggle ();
			}
		}
	}
}