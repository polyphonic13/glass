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
			target.SetActive (true);
		}

		public void Disable() {
			target.SetActive (false);
		}
	}
}