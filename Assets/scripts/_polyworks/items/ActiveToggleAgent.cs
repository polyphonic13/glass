namespace Polyworks {
	using UnityEngine;
	using System.Collections;

	public class ActiveToggleAgent : Toggler
	{
		public override void Toggle ()
		{
			base.Toggle ();
			_toggle ();
		}

		private void Awake() {
			_toggle ();
		}

		private void _toggle() {
			this.gameObject.SetActive (isOn);
		}
	}
}