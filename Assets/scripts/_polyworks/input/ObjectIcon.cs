namespace Polyworks {
	using UnityEngine;
	using System.Collections;

	public class ObjectIcon : MonoBehaviour
	{
		public GameObject target;

		public void SetTarget(GameObject target) {
			this.target = target;
		}

		public void Enable() {
			if (target != null) {
				target.SetActive (true);
			}
		}

		public void Disable() {
			if (target != null) {
				target.SetActive (false);
			}
		}
	}
}