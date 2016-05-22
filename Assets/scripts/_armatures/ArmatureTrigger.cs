using UnityEngine;
using Polyworks; 

public class ArmatureTrigger : Item {
	public ArmatureParent _pops;
	public Transform _parentBone;
	public AnimationClip _mainClip;

	public bool isLooping = false;

	public void OnActuate() {
		if(isEnabled) {
			Actuate();
		}
	}

	public override void Actuate() {
//		Debug.Log ("ArmatureTrigger[" + this.name + "]/Actuate");
		HandleAnimation();
	}

	public virtual void HandleAnimation() {
		SendAnimationToPops(_mainClip.name, _parentBone);
	}
	
	public void SendAnimationToPops(string clipName, Transform bone = null) {
		_pops.PlayAnimation(clipName, bone, isLooping);
	}
}
