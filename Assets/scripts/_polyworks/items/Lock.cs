using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class Lock : Item
	{
		public bool isLocked;

		public string eventType;
		public string eventValue;

		private Switch[] _switches;
		private bool _isInSection = false;

		public void OnStringEvent(string type, string value) {
			if (type == eventType && value == eventValue) {
				if (isLocked) {
					isLocked = false;
					if (_isInSection) {
						base.Enable ();
					}
				}
			}
		}

		public override void Enable() {
			Debug.Log ("Lock[" + this.name + "]/Enable, isLocked = " + isLocked + ", isEnabled = " + isEnabled);
			_isInSection = true;
			if (!isLocked) {
				base.Enable ();
			}
		}

		public override void Disable () {
			_isInSection = false;
			if (!isLocked) {
				base.Disable ();
			}
		}

		public override void Actuate () {
			Debug.Log ("Lock[" + this.name + "]/Actuate, isLocked = " + isLocked + ", isEnabled = " + isEnabled);
			if (isEnabled) {
				if (!isLocked) {
					base.Actuate ();
					_actuate ();
				} else {
					EventCenter.Instance.AddNote (this.displayName + " is locked");
				}
			}
		}

		private void Awake() {
			_switches = gameObject.GetComponents<Switch> ();
			EventCenter.Instance.OnStringEvent += OnStringEvent;
		}

		private void _actuate() {
			if (_switches != null) {
				for (int i = 0; i < _switches.Length; i++) {
					if (_switches [i] != null) {
						_switches [i].Actuate ();
					}
				}
			}
		}
		private void OnDestroy() {
			EventCenter ec = EventCenter.Instance;
			if (ec != null) {
				EventCenter.Instance.OnStringEvent -= OnStringEvent;
			}
		}
	}
}

