namespace Polyworks {
	using UnityEngine;
	using System.Collections;

	public class PuzzleComponent : MonoBehaviour
	{
		public GameObject collider;
		public GameObject highlight; 

		private void Awake() {
			if (highlight != null) {
				_activateHighlight(false);
			}
		}

		private void _activateHighlight(bool isActivated) {
			highlight.SetActive (isActivated);
		}
	}
}
