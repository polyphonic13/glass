using UnityEngine;

public class OpenCloseArmatureTrigger : ArmatureTrigger {

	public AnimationClip _closeClip;

	public bool IsOpen { get; set; }

	void Awake() {
		InitOpenCloseArmatureTrigger();
		Init();
	}
	
	public void InitOpenCloseArmatureTrigger() {
		_pops.OnAnimationPlayed += OnAnimationPlayed;
		IsOpen = false;
	}
	
	public override void HandleAnimation() {
		HandleOpenClose();
	}
	
	public void HandleOpenClose() {
		Debug.Log("OpenCloseArmatureChild[ " + name + " ]/HandleOpenClose, IsOpen = " + IsOpen);
		if(IsOpen) {
			Debug.Log("close name = " + _closeClip.name);
			SendAnimationToPops(_closeClip.name, _parentBone);
		} else {
			Debug.Log("main name = " + _mainClip.name);
			SendAnimationToPops(_mainClip.name, _parentBone);
		}
		IsOpen = !IsOpen;
	}
	
	public void OnAnimationPlayed(Transform bone) {
		if(IsOpen) {
//			Debug.Log("OpenCloseArmatureTrigger[ " + name + " ]/onAnimationPlayed, IsOpen = " + IsOpen + ", bone = " + bone.name + ", parentBone = " + _parentBone.name);
			if(bone.name != _parentBone.name) {
				SendAnimationToPops(_closeClip.name, _parentBone);
				IsOpen = false;
			}
		}
	}
	
}
