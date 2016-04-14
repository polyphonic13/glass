using UnityEngine;
using System.Collections;

public class MinerRobot : MonoBehaviour {

	public AnimationClip[] movementAnimations;
	public Transform[] movementBones; 

	private int _currentMovement = -1;

	private Animation _animation; 

	public void StartMovement() {
		Debug.Log ("MinerRobot/PlayNextMovement, _currentMovement = " + _currentMovement + " length = " + movementAnimations.Length);

		if (_currentMovement < movementAnimations.Length - 1) {
			_currentMovement++;
		} else {
			_currentMovement = 0;
		}
		string clip = movementAnimations [_currentMovement].name;
		Transform bone = movementBones [_currentMovement];
		Debug.Log ("clip = " + clip);
//		if(bone != null) {
//			_animation [clip].AddMixingTransform(bone);
//		}
		_animation [clip].wrapMode = WrapMode.Loop;
		_animation.Play(clip);
	}

	public void StartHands() {

	}

	public void StopHands() {

	}

	void Awake() {
		_animation = GetComponent<Animation> ();
		StartMovement();
	}
	
	void Update () {
	
	}

}
