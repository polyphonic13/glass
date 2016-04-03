using UnityEngine;
using System.Collections;

public class ArmatureController : TargetController {

	public ArmatureParent target;
	public AnimationClip unlockClip; 
	public AnimationClip closeClip;

	public override void Actuate() {
		if (closeClip != null) {
//			Debug.Log ("ArmatureController/Actuate, closeClip.name = " + closeClip.name + ", isOpen = " + target.isOpen);
			if (!target.isOpen) {
				target.PlayAnimation (unlockClip.name);
			} else {
				target.PlayAnimation (closeClip.name);
			}
		} else {
			target.PlayAnimation (unlockClip.name);
		}
	}

	public override bool GetIsActive() {
		return target.GetIsActive ();
	}
}
