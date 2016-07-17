using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class MenuUI : UIController
	{
		private void Awake() {
			base.Init ();
		}

		private void FixedUpdate() {
			if (canvas.enabled) {
				if (cancel) {
					cancel = false;
					SetActive (false);
				}
			}
		}
	}
}


