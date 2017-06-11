using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class BoolEventAgent : MonoBehaviour {
		public string eventType = "";
		public bool eventValue = false; 

		public void OnBoolEvent(string type, bool value) {
			//			Debug.Log ("BoolEventAgent[" + this.name + "]/OnBoolEvent, type = " + type + ", eventType = " + eventType + ", value = " + value + ", eventValue = " + eventValue);
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
			EventCenter ec = EventCenter.Instance;
			if (ec != null) {
				ec.OnBoolEvent += OnBoolEvent;
			}
		}

		private void _removeListeners() {
			EventCenter ec = EventCenter.Instance;
			if (ec != null) {
				ec.OnBoolEvent -= OnBoolEvent;
			}
		}
	}
}
