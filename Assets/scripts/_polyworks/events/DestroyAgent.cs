using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class DestroyAgent : MonoBehaviour
	{
		public void OnDestroyEvent(string target) {
			Kill (target);
		}

		public void Kill(string target) {
			if (target == this.name) {
				Destroy (gameObject);
			}
		}

		public void Enable() {
			_addListeners ();
		}

		public void Disable() {
			_removeListeners ();
		}

		private void OnDestroy() {
			_removeListeners ();
		}

		private void _addListeners() {
			EventCenter ec = EventCenter.Instance;
			if (ec != null) {
				ec.OnDestroyEvent += OnDestroyEvent;
			}
		}

		private void _removeListeners() {
			EventCenter ec = EventCenter.Instance;
			if (ec != null) {
				ec.OnDestroyEvent -= OnDestroyEvent;
			}
		}
	}
}

