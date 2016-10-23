using UnityEngine;
using UnityEngine.UI;

namespace Polyworks {
	public class MenuUI : UIController
	{
		public Button[] buttons;

		private int _btnIndex = 0;

		private void Awake() {
			base.Init ();
		}

		private void FixedUpdate() {
//			Debug.Log("MenuUI/FixedUpdate, canvas.enabled = " + canvas.enabled);
			if (canvas.enabled) {
				if (cancel) {
					cancel = false;
					SetActive (false);
				} else if(down) {
					if(_btnIndex < buttons.Length - 1) {
						_btnIndex++;
					} else {
						_btnIndex = 0;
					}
				} else if(up) {
					if(_btnIndex > 0) {
						_btnIndex--;
					} else {
						_btnIndex = buttons.Length - 1;
					}
				} else if(confirm) {
					Debug.Log("confirm");
					buttons[_btnIndex].onClick.Invoke();
				}

			}
		}
	}
}


