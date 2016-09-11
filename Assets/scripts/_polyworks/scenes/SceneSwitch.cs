﻿using UnityEngine;
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

		public void Actuate() {
//			Game.Instance.ChangeScene(targetScene, targetSection);
			EventCenter.Instance.ChangeScene(targetScene, targetSection);
		}
	}
}
