using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class SceneChangeSwitch : Switch
	{
		public string targetScene;
		public int targetRoom = -1;

		private bool isActive { get; set; }

		public void SetActive(bool active) {
			isActive = active;
		}

		public void Actuate() {
			Game.Instance.ChangeScene(targetScene);
		}
	}
}
