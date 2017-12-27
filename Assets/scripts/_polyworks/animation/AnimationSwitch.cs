using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class AnimationSwitch : Switch
	{
		public string targetName; 
		public string[] animations; 
		public bool isLogOn = false;

		public int currentIdx { get; set; }

		private AnimationAgent _target; 

		public override void Actuate() {
//			_log ("AnimationSwitch[" + this.name + "]/Actuate, _target = " + _target);
			if (_target != null) {
				if (animations.Length > 0) {
//					_log (" sending current["+currentIdx+"] animation: " + animations [currentIdx]);
					_target.Play (animations [currentIdx]);
					_incrementIndex ();
				} else {
//					_log (" going to call _target.Play");
					_target.Play ("");
				}
			}
		}

		private void Awake () {
			currentIdx = 0;
			GameObject targetObject = GameObject.Find (targetName);
			if (targetObject != null) {
				_target = targetObject.GetComponent<AnimationAgent> ();
//				_log ("AnimationSwitch[" + this.name + "]/Awake, targetObject = " + targetObject + ", _target = " + _target);
			}
		}

		private void _incrementIndex() {
			if (currentIdx < animations.Length - 1) {
				currentIdx++;
			} else {
				currentIdx = 0;
			}
		}

		private void _log(string message) {
			if (isLogOn) {
				Debug.Log (message);
			}
		}
	}
}
