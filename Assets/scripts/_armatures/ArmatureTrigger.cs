using UnityEngine;

public class ArmatureTrigger : InteractiveItem {
	public ArmatureParent _pops;
	public Transform _parentBone;
	public AnimationClip _mainClip;

	void Awake() {
		Init();
	}
	
	public void OnActuate() {
		if(IsRoomActive && IsEnabled) {
			Actuate();
		}
	}

	public override void Actuate() {
		HandleAnimation();
	}

	public virtual void HandleAnimation() {
		SendAnimationToPops(_mainClip.name, _parentBone);
	}
	
	public void SendAnimationToPops(string clipName, Transform bone = null) {
		_pops.PlayAnimation(clipName, bone);
	}
}
