using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class ExaminableItem : Item {

		public bool isSingleUse = true;

		private bool _isUsedOnce = false;

		public override void Enable () {
			Debug.Log ("ExaminableItem[" + this.name + "]/Enable");
			base.Enable ();
		}

		public override void Actuate() {
			Debug.Log ("ExaminableItem[" + this.name + "]/Actuate");
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
