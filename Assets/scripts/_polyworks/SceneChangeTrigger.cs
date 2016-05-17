using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class SceneChangeTrigger : MonoBehaviour
	{
		public string targetScene;
		public int targetRoom;

		private bool isActive { get; set; }

		void Awake() {
//			isActive = false;
		}

		void OnTriggerEnter(Collider tgt) {
			//		Debug.Log ("SceneChangeTrigger/OnTriggerEnter, tgt = " + tgt.gameObject.tag + ", isActive = " + isActive);
			if(isActive) {
				//			Debug.Log("scene changer trigger, tgt.tag = " + tgt.gameObject.tag);
				if(tgt.gameObject.tag == "Player") {
					Trigger ();
				}
			}
		}

		public void SetActive(bool active) {
			//		Debug.Log ("SceneChangeTrigger/SetActive, active = " + active);
			isActive = active;
		}

		public void Trigger() {
			GameController.Instance.ChangeScene(targetScene);
		}
	}
}
