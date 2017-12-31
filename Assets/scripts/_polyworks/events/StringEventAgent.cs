using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class StringEventAgent : MonoBehaviour {
		public string eventType = "";
		public string eventValue = ""; 

		private bool _isListenersAdded = false;

		public void OnStringEvent(string type, string value) {
//			Debug.Log ("StringEventAgent[" + this.name + "]/OnStringEvent, type = " + type + ", eventType = " + eventType + ", value = " + value + ", eventValue = " + eventValue);
			if (type == eventType && value == eventValue) {
				SendMessage ("Actuate", null, SendMessageOptions.DontRequireReceiver);
			}
		}

		public void Enable() {
			_addListeners ();
		}

		public void Disable() {
			_removeListeners ();
		}

		private void Awake() {
			_addListeners ();
		}

		private void OnDestroy() {
			_removeListeners ();
		}

		private void _addListeners() {
			if (!_isListenersAdded) {
				_isListenersAdded = true;
				EventCenter ec = EventCenter.Instance;
				if (ec != null) {
					ec.OnStringEvent += OnStringEvent;
				}
			}
		}

		private void _removeListeners() {
			if (_isListenersAdded) {
				_isListenersAdded = false;
				EventCenter ec = EventCenter.Instance;
				if (ec != null) {
					ec.OnStringEvent -= OnStringEvent;
				}
			}
		}
	}
}
