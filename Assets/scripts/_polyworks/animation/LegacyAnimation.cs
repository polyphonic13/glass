using UnityEngine;
using System.Collections;

namespace Polyworks {
	[System.Serializable]
	public struct AnimationBone {
		public string animation;
		public Transform bone;
	}

	[System.Serializable]
	public class AnimationBoneCollection {
		public AnimationBone[] animationBones;

		public static Transform GetBone(string name, AnimationBone[] bones) {
			Transform bone = null;
			for (int i = 0; i < bones.Length; i++) {
				if (name == bones[i].animation) {
					bone = bones[i].bone;
					break;
				}
			}
			return bone;
		}
	}

	public class LegacyAnimation : AnimationAgent {

		public AnimationClip[] animationClips;
		public AnimationBoneCollection bones; 

		public bool isAutoStart = false; 
		public bool isAutoAdvance = true;
		public bool isLoopPlayback = false;

		private const float PLAY_SPEED = 1f;
		private const float PAUSE_SPEED = 0f; 

		private int _currentAnimation = 0;

		private Animation _animation; 
		private bool _isPlaying = false; 

		public override void Play(string clip = "") {
			Actuate (clip);
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
//			Debug.Log ("LegacyAnimation[" + this.name + "]/Actuate, clip = " + clip);
			string c;
			if (clip == "") {
				c = animationClips [_currentAnimation].name;
//				Debug.Log (" no clip param, going to play " + c);
				_incrementCurrentAnimation ();
			} else {
				c = clip;
			}

			Transform bone = AnimationBoneCollection.GetBone (c, bones.animationBones);

			if (bone != null) {
//				Debug.Log (" THERE IS A BONE: " + bone);
				_animation [c].AddMixingTransform(bone);
			}

			_animation [c].wrapMode = WrapMode.Once;
			_animation [c].speed = PLAY_SPEED;
//			Debug.Log (" about to call _animation Play on " + c + ", _animation = " + _animation + ", clip = " + _animation[c]);
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
