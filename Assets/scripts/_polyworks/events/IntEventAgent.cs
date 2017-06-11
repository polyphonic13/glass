using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class IntEventAgent : MonoBehaviour {
		public string eventType = "";
		public int eventValue = -1; 

		public void OnIntEvent(string type, int value) {
			Debug.Log ("IntEventAgent[" + this.name + "]/OnIntEvent, type = " + type + ", eventType = " + eventType + ", value = " + value + ", eventValue = " + eventValue);
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
				ec.OnIntEvent += OnIntEvent;
			}
		}

		private void _removeListeners() {
			EventCenter ec = EventCenter.Instance;
			if (ec != null) {
				ec.OnIntEvent -= OnIntEvent;
			}
		}
	}
}
