using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class UIController : MonoBehaviour
	{
		public bool isActiveOnAwake = false;

		public Canvas canvas;

		public float horizontal { get; set; }
		public float vertical { get; set; }

		public bool up { get; set; }
		public bool down { get; set; } 
		public bool left { get; set; } 
		public bool right { get; set; }

		public bool confirm { get; set; }
		public bool cancel { get; set; }

		#region public methods
		public virtual void Init() {
			canvas.enabled = isActiveOnAwake;
		}

		public virtual void SetActive(bool isActive) {
			canvas.enabled = isActive;
		}

		public void SetHorizontal(float h) {
			horizontal = h;
		}

		public void SetVertical(float v) {
			vertical = v;
		}

		public void SetUp(bool isPressed) {
			up = isPressed;
		}

		public void SetDown(bool isPressed) {
			down = isPressed;
		}

		public void SetLeft(bool isPressed) {
			left = isPressed;
		}

		public void SetRight(bool isPressed) {
			right = isPressed;
		}

		public void SetConfirm(bool isPressed) {
			confirm = isPressed;
		}

		public void SetCancel(bool isPressed) {
			// Debug.Log ("UIController[" + this.name + "]/SetCancel, isPressed = " + isPressed);
			cancel = isPressed;
		}
		#endregion

	}

}

