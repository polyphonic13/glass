using UnityEngine;
using System.Collections;

public class LegacyAnimationController : MonoBehaviour {

	public AnimationClip[] animationClips;
	public bool isAutoStart = false; 
	public bool loopAnimations = false;

	private const float PLAY_SPEED = 1f;
	private const float PAUSE_SPEED = 0f; 

	private int _currentAnimation = 0;

	private Animation _animation; 
	private bool _isPlaying = false; 

	public void StartAnimation() {
		string clip = animationClips [_currentAnimation].name;
		_animation [clip].wrapMode = WrapMode.Loop;
		_animation [clip].speed = PLAY_SPEED;
		_animation.Play(clip);
		_isPlaying = true;
	}

	public void AnimationEnded() {
		if (_currentAnimation < animationClips.Length - 1) {
			_currentAnimation++;
			StartAnimation ();
		} else if (loopAnimations) {
			_currentAnimation = 0;
			StartAnimation ();
		} else {
			_isPlaying = false;
		}
	}

	public void Pause() {
		_adjustSpeed (PAUSE_SPEED);
	}

	public void Resume() {
		_adjustSpeed (PLAY_SPEED);
	}

	public bool GetIsPlaying() {
		return _animation.isPlaying;
	}

	private void Awake() {
		_animation = GetComponent<Animation> ();
		if (isAutoStart) {
			StartAnimation ();
		}
	}

	private void _adjustSpeed(float speed) {
		string clip = animationClips [_currentAnimation].name;
		_animation [clip].speed = speed;
	}
}
