using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArmatureTrigger : InteractiveElement {
	public ArmatureParent pops;
	public Transform parentBone;
	public AnimationClip mainClip;

	void Awake() {
		init();
	}
	
	public void OnMouseDown() {
		this.InputTaken ();
	}

	public override void InputTaken() {
//		Debug.Log ("ArmatureTrigger[" + this.name + "]/InputTaken, isRoomActive = " + this.isRoomActive + ", enabled = " + this.enabled);
//		if(this.isRoomActive && this.isEnabled) {
			var difference = Vector3.Distance(playerHead.position, this.transform.position);
			if(difference <= interactDistance) {
					handleAnimation();
			}
//		} 
	}

	public virtual void handleAnimation() {
//		Debug.Log("ArmatureTrigger[ " + this.name + " ]/handleAnimation");
		sendAnimationToPops(mainClip.name, parentBone);
	}
	
	public void sendAnimationToPops(string clipName, Transform bone = null) {
//		Debug.Log("ArmatureTrigger[ " + this.name + " ]/sendAnimationToPops, clipName = " + clipName);
		pops.playAnimation(clipName, bone);
	}
}
