using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class AnimationSwitch : MonoBehaviour
	{
		public string targetName; 
		public string[] animations; 
		public int currentIdx { get; set; }

		private AnimationAgent _target; 

		public void Actuate() {
			if (_target != null) {
				_target.Play (animations[currentIdx]);
				_incrementIndex();
			}
		}

		private void Awake () {
			currentIdx = 0;
			GameObject targetObject = GameObject.Find (targetName);
			if (targetObject != null) {
				_target = targetObject.GetComponent<AnimationAgent> ();
			}
		}

		private void _incrementIndex() {
			if (currentIdx < animations.Length - 1) {
				currentIdx++;
			} else {
				currentIdx = 0;
			}
		}
	}
}
