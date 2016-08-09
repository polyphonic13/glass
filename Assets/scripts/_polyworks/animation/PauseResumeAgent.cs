using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class PauseResumeAgent : MonoBehaviour {
		public Animation target; 
		
		public virtual void Enable() {
			_send("Resume");
		}

		public virtual void Disable() {
			_send("Pause");
		}
		
		private void Awake() {
			if(target == null) {
				target = GetComponent<Animation>();
			}
		}
		
		private void _send(string message) {
			if(target == null) {
				return;
			}
			if (GetIsActive ()) {
				gameObject.SendMessage(message);
			}
		}
	}
}