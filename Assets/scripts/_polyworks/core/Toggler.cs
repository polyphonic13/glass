namespace Polyworks {
	using UnityEngine;
	using System.Collections;

	public class Toggler : MonoBehaviour {

		public bool isOn = false;

		public virtual void Toggle() {
			isOn = !isOn;
		}

		public virtual void ToggleTarget(bool turnOn) {
			isOn = turnOn;
		}
	}
}
