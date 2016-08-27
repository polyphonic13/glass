using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class ExaminableItem : Item {

		public string description = "";
		public bool isSingleUse = true;

		private bool _isUsedOnce = false;

		public override void Actuate() {
			if(isEnabled) {
				if(!isSingleUse || !_isUsedOnce) {
					_isUsedOnce = true;
					EventCenter.Instance.AddNote(description);
					if (isSingleUse) {
						Destroy (this.gameObject);
						EventCenter.Instance.ChangeItemProximity (this, false);
					}
				}
			}
		}
	}
}
