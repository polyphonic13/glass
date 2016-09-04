﻿using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class SwitchController : Item
	{
		#region public members
		public bool isLocked;
		public bool isLockMessageDisplayed = true; 

		public string eventType;
		public string eventValue;
		#endregion

		#region private members
		private Switch[] _switches;
		private bool _isInSection = false;
		#endregion

		#region handlers
		public void OnStringEvent(string type, string value) {
			if (type == eventType && value == eventValue) {
				if (isLocked) {
					isLocked = false;
				}
			}
		}
		#endregion

		#region public methods
		public override void Actuate () {
			Debug.Log ("SwitchController[" + this.name + "]/Actuate, _switches = " + _switches.Length);
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

			if (isLocked) {	
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