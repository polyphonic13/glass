using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class SwitchController : Item
	{
		#region public members
		public bool isLocked = false;
		public bool isLockMessageDisplayed = true;
		public bool isAutoActuatedOnUnlock = false;
		public bool isNoteAddedOnUnlock = true;

		public string eventType = "";
		public string eventValue = "";
		#endregion

		#region private members
		private Switch[] _switches;
		private bool _isInSection = false;
		#endregion

		#region handlers
		public void OnStringEvent(string type, string value) {
//			Debug.Log ("SwitchController[" + this.name + "]/OnStringEvent, type " + type + ", eventType = " + eventType);
//			Debug.Log(" value = " + value + ", eventValue = " + eventValue);
			if (type == eventType && value == eventValue) {
//				Debug.Log (" is a MATCH");
				if (isLocked) {
					isLocked = false;
					if (isNoteAddedOnUnlock) {
						EventCenter.Instance.AddNote ("The " + this.displayName + " was unlocked");
					}
					if (isAutoActuatedOnUnlock) {
						Actuate ();
					}
				}
			}
		}
		#endregion

		#region public methods
		public override void Actuate () {
			Debug.Log ("SwitchController[" + this.name + "]/Actuate, isLocked = " + isLocked + ", _switches = " + _switches.Length);
			if (!isLocked) {
				base.Actuate ();
				_actuate ();
			} else if(isLockMessageDisplayed) {
				EventCenter.Instance.AddNote ("The " + this.displayName + " is locked");
			}
		}
		#endregion

		#region private methods
		private void Awake() {
			if (transform.tag == "persistent") {
				Enable ();
			}
			_switches = gameObject.GetComponents<Switch> ();

			if (isLocked && eventType != "") {	
				EventCenter.Instance.OnStringEvent += OnStringEvent;
			}
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
		#endregion
	}
}