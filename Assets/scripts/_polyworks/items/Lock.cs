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
//			Debug.Log ("Lock[" + this.name + "]/OnStringEvent, type = " + type + ", eventType = " + eventType + ", value = " + value + ", eventValue = " + eventValue);
			if (type == eventType && value == eventValue) {
				if (isLocked) {
					isLocked = false;
					EventCenter.Instance.AddNote ("The " + this.displayName + " was unlocked");
					Actuate ();
				}
			}
		}

		public override void Actuate () {
//			Debug.Log ("Lock[" + this.name + "]/Actuate, isLocked = " + isLocked + ", isEnabled = " + isEnabled);
			if (!isLocked) {
				base.Actuate ();
				_actuate ();
			} else {
				EventCenter.Instance.AddNote ("The " + this.displayName + " is locked");
			}
		}

		private void Awake() {
			if (transform.tag == "persistent") {
				Enable ();
			}
			_switches = gameObject.GetComponents<Switch> ();
			EventCenter.Instance.OnStringEvent += OnStringEvent;
		}

		private void _actuate() {
//			Debug.Log ("Lock[" + this.name + "]/_actuate, _switches = " + _switches);
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

