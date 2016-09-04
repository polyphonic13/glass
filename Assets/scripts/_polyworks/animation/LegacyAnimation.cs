using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class LegacyAnimation : AnimationAgent {

		public AnimationClip[] animationClips;

		public bool isAutoStart = false; 
		public bool isAutoAdvance = true;
		public bool isLoopPlayback = false;

		private const float PLAY_SPEED = 1f;
		private const float PAUSE_SPEED = 0f; 

		private int _currentAnimation = 0;

		private Animation _animation; 
		private bool _isPlaying = false; 

		public override void Play(string clip = "") {
			Actuate ();
		}

		public override void Pause() {
			_adjustSpeed (PAUSE_SPEED);
		}

		public override void Resume() {
			_adjustSpeed (PLAY_SPEED);
		}

		public override bool GetIsActive() {
			if (_animation == null) {
				return false;
			}
			return _animation.isPlaying;
		}

		public void Actuate(string clip = "") {
			string c;
			if (clip == "") {
				c = animationClips [_currentAnimation].name;
				_incrementCurrentAnimation ();
			} else {
				c = clip;
			}
			_animation [c].wrapMode = WrapMode.Once;
			_animation [c].speed = PLAY_SPEED;
			_animation.Play(c);
			_isPlaying = true;

		}

		private void Awake() {
			_animation = GetComponent<Animation> ();
			if (isAutoStart) {
				Actuate ();
			}
		}

		private void _incrementCurrentAnimation() {
			if (_currentAnimation < animationClips.Length - 1) {
				_currentAnimation++;
			} else {
				_currentAnimation = 0;
			}
		}

		private void _adjustSpeed(float speed) {
//			Debug.Log ("LegacyAnimation[" + this.name + "]/_adjustSpeed, speed = " + speed);
			string clip = animationClips [_currentAnimation].name;
			_animation [clip].speed = speed;
		}
	}
}
