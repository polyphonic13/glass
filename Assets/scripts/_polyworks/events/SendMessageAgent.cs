namespace Polyworks {
	using UnityEngine;
	using System;
	using System.Collections;

	public class SendMessageAgent : MonoBehaviour
	{
		public string method;
		public string type;

		public void OnStringEvent(string type, string value) {
			Debug.Log ("SendMessageAgent[" + this.name + "]/OnStringEvent, type = " + type + ", this.type = " + this.type);
			if (type == this.type) {
				SendMessage (method, value, SendMessageOptions.DontRequireReceiver);
			}
		}

		public void OnIntEvent(string type, int value) {
			Debug.Log ("SendMessageAgent[" + this.name + "]/OnIntEvent, type = " + type + ", this.type = " + this.type);
			if (type == this.type) {
				SendMessage (method, value, SendMessageOptions.DontRequireReceiver);
			}
		}

		private void Awake() {
			EventCenter.Instance.OnStringEvent += OnStringEvent;
			EventCenter.Instance.OnIntEvent += OnIntEvent;
		}

		private void OnDestroy() {
			EventCenter ec = EventCenter.Instance;
			if (ec != null) {
				ec.OnStringEvent -= OnStringEvent;
				ec.OnIntEvent -= OnIntEvent;
			}
		}
	}
}
