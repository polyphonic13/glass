using UnityEngine;

namespace Polyworks {
	public class ArmatureAgent : AnimationAgent {
	
		public delegate void AnimationHandler(Transform bone);
	
		public event AnimationHandler OnAnimationPlayed;
	
		public AnimationClip defaultAnimation; 
	
		private Animation _animation;
		private string _currentClip; 

		public bool isOpen { get; set; }

		public override void Play(string clip = "") {
//			Debug.Log ("ArmatureAgent[" + this.name + "]/Play, clip = " + clip + ", isPlaying = " + _animation.isPlaying);
			if(!_animation.isPlaying) {
				base.Play ();
				if (clip == "") {
					PlayDefaultAnimation ();
				} else {
					PlayAnimation (clip);
				}
			}
		}
		
		public override void Pause() {
			if (_animation.isPlaying) {
				base.Pause ();
				_animation [_currentClip].speed = 0;
			}
		}

		public override void Resume() {
			if (!_animation.isPlaying) {
				base.Resume ();
				_animation [_currentClip].speed = 1;
			}
		}

		public override bool GetIsActive() { 
			return _animation.isPlaying;
		}

		public virtual void PlayAnimation(string clip, Transform bone = null, bool isLooping = false) {
			isOpen = !isOpen;
			AnimateArmatureBone(clip, bone, isLooping);
		}
	
		public void AnimateArmatureBone(string clip, Transform bone = null, bool isLooping = false) {
			if (_animation [clip] != null) {
				_currentClip = clip;

				if(bone != null) {
					_animation [clip].AddMixingTransform(bone);
				}
				if (isLooping) {
					_animation [clip].wrapMode = WrapMode.Loop;
				} else {
					_animation [clip].wrapMode = WrapMode.Once;
				}
				_animation.Play(clip);
			}
		}

		public virtual void Init() {
			isOpen = false;
			_animation = GetComponent<Animation>();
			PlayDefaultAnimation();

		}
	
		public void PlayDefaultAnimation() {
			if(defaultAnimation != null) {
				_animation [defaultAnimation.name].layer = 0;
				_animation[defaultAnimation.name].wrapMode = WrapMode.Once;
				_animation.Play(defaultAnimation.name);
			}
		}
	
		public void AnimationPlayed(Transform bone = null) {
			if(OnAnimationPlayed != null) {
				OnAnimationPlayed(bone);
			}
		}

		private void Awake() {
			Init();
		}
	}
}
