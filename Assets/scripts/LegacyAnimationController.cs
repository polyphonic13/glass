﻿using UnityEngine;
using System.Collections;

public class LegacyAnimationController : TargetController {

	public AnimationClip[] animationClips;
	public bool isAutoStart = false; 
	public bool isAutoAdvance = true;
	public bool isLoopPlayback = false;

	private const float PLAY_SPEED = 1f;
	private const float PAUSE_SPEED = 0f; 

	private int _currentAnimation = 0;

	private Animation _animation; 
	private bool _isPlaying = false; 

	public override void Actuate() {
		string clip = animationClips [_currentAnimation].name;
		_animation [clip].wrapMode = WrapMode.Loop;
		_animation [clip].speed = PLAY_SPEED;
		_animation.Play(clip);
		_isPlaying = true;
	}

	public override void Pause() {
		Debug.Log ("LegacyAnimationController[" + this.name + "]/Pause");
		_adjustSpeed (PAUSE_SPEED);
	}

	public override void Resume() {
		Debug.Log ("LegacyAnimationController[" + this.name + "]/Resume");
		_adjustSpeed (PLAY_SPEED);
	}

	public override bool GetIsActive() {
		return _animation.isPlaying;
	}

	public void AnimationEnded() {
		if (isAutoAdvance) {
			if (_currentAnimation < animationClips.Length - 1) {
				_currentAnimation++;
			} else {
				_currentAnimation = 0;
			}
			Actuate ();
		} else {
			_isPlaying = false; 
		}
	}

	private void Awake() {
		_animation = GetComponent<Animation> ();
		if (isAutoStart) {
			Actuate ();
		}
	}

	private void _adjustSpeed(float speed) {
		string clip = animationClips [_currentAnimation].name;
		_animation [clip].speed = speed;
	}
}
