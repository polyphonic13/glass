using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class SceneSwitch : Switch
	{
		public string targetScene;
		public int targetSection = -1;

		private bool isActive { get; set; }

		public void SetActive(bool active) {
			isActive = active;
		}

		public override void Actuate() {
			Debug.Log ("SceneSwitch[" + this.name + "]/Actuate, targetScene = " + targetScene + ", targetSection = " + targetSection);
			EventCenter.Instance.StartSceneChange(targetScene, targetSection);
		}
	}
}
