using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class SceneChangeTrigger : MonoBehaviour
	{
		public string targetScene;
		public int targetRoom;

		private bool isActive { get; set; }

		void OnTriggerEnter(Collider tgt) {
			if(isActive) {
				if(tgt.gameObject.tag == "Player") {
					Trigger ();
				}
			}
		}

		public void SetActive(bool active) {
			isActive = active;
		}

		public void Trigger() {
			Game.Instance.ChangeScene(targetScene);
		}
	}
}
