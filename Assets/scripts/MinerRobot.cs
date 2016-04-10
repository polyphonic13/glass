using UnityEngine;
using System.Collections;

public class MinerRobot : MonoBehaviour {

	public AnimationClip[] movementAnimations;
	public Transform[] movementBones; 

	private int _currentMovement = 0;

	private Animation _animation; 

	public void PlayNextMovement() {
		if (_currentMovement < movementAnimations.Length - 1) {
			_currentMovement++;
		} else {
			_currentMovement = 0;
		}
		string clip = movementAnimations [_currentMovement].name;
		Transform bone = movementBones [_currentMovement];
		Debug.Log ("MinerRobot/PlayNextMovement, clip = " + clip);
		if(bone != null) {
			_animation [clip].AddMixingTransform(bone);
		}
		_animation [clip].wrapMode = WrapMode.Once;
		_animation.Play(clip);
	}

	void Awake() {
		_animation = GetComponent<Animation> ();
		PlayNextMovement ();
	}
	
	void Update () {
	
	}

}
