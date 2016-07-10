using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class UIController : MonoBehaviour
	{
		public Canvas canvas;

		public float horizontal;
		public float vertical; 

		#region public methods
		public virtual void Init() {
			canvas = gameObject.transform.parent.GetComponent<Canvas>();
		}

		public void SetActive(bool isActive) {
			canvas.enabled = isActive;
		}

		public void SetHorizontal(float h) {
			horizontal = h;
		}

		public void SetVertical(float v) {
			vertical = v;
		}
		#endregion

	}

}

