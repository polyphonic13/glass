using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class ExaminableItem : Item {

		public bool isSingleUse = true;

		private bool _isUsedOnce = false;

		public override void Enable () {
			base.Enable ();
		}

		public override void Actuate() {
			if(isEnabled) {
				if(!isSingleUse || !_isUsedOnce) {
					EventCenter ec = EventCenter.Instance;

					_isUsedOnce = true;
					ec.AddNote(description);

					if (isSingleUse) {
						Destroy (this.gameObject);
						ec.ChangeItemProximity (this, false);
					}
				}
			}
		}
	}
}
