using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class PlayerHead : ItemDetectionRaycastAgent {

		public void OnContextChange(InputContext context, string param) {
			if (context == InputContext.PLAYER) {
				this.isActive = true;
			} else if(this.isActive) {
				this.isActive = false;
			}
		}

		private void Awake() {
			this.detectionDistance = 4f;
			this.dynamicTag = "interactive";
			this.staticTag = "persistent";

			EventCenter ec = EventCenter.Instance;
			if (ec != null) {
				ec.OnContextChange += this.OnContextChange;
			}
		}

		private void OnDestroy() {
			EventCenter ec = EventCenter.Instance;
			if (ec != null) {
				ec.OnContextChange -= this.OnContextChange;
			}
		}

		private void Update() {
			if (this.isActive) {
				CheckRayCast ();
			}
		}
	}
}
