using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class AnimationSwitch : Switch
	{
		public string targetName; 
		public string[] animations; 
		public int currentIdx { get; set; }

		private AnimationAgent _target; 

		public override void Actuate() {
			Debug.Log ("AnimationSwitch[" + this.name + "]/Actuate, _target = " + _target);
			if (_target != null) {
				if (animations.Length > 0) {
					Debug.Log (" sending current["+currentIdx+"] animation: " + animations [currentIdx]);
					_target.Play (animations [currentIdx]);
					_incrementIndex ();
				} else {
//					Debug.Log (" going to call _target.Play");
					_target.Play ("");
				}
			}
		}

		private void Awake () {
			currentIdx = 0;
			GameObject targetObject = GameObject.Find (targetName);
			if (targetObject != null) {
				_target = targetObject.GetComponent<AnimationAgent> ();
//				Debug.Log ("AnimationSwitch[" + this.name + "]/Awake, targetObject = " + targetObject + ", _target = " + _target);
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
