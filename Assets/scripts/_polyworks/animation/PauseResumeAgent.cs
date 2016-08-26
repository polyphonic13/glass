using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class PauseResumeAgent : MonoBehaviour {
		public AnimationAgent target; 
		
		public virtual void Enable() {
			_send("Resume");
		}

		public virtual void Disable() {
			_send("Pause");
		}
		
		private void Awake() {
			if(target == null) {
				target = GetComponent<AnimationAgent>();
			}
		}
		
		private void _send(string message) {
			if(target == null) {
				return;
			}
			if (target.GetIsActive ()) {
				gameObject.SendMessage(message, null, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}