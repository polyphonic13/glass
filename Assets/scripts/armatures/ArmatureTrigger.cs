using UnityEngine;

public class ArmatureTrigger : InteractiveElement {
	public ArmatureParent _pops;
	public Transform _parentBone;
	public AnimationClip _mainClip;

	void Awake() {
		Init();
	}
	
	public void OnInputTaken() {
		InputTaken();
	}

	public override void InputTaken() {
//		Debug.Log ("ArmatureTrigger[" + name + "]/InputTaken, IsRoomActive = " + IsRoomActive + ", enabled = " + enabled);
//		if(IsRoomActive && _isEnabled) {
			var difference = Vector3.Distance(PlayerHead.position, transform.position);
			if(difference <= _interactDistance) {
				HandleAnimation();
			}
//		} 
	}

	public virtual void HandleAnimation() {
//		Debug.Log("ArmatureTrigger[ " + name + " ]/HandleAnimation");
		SendAnimationToPops(_mainClip.name, _parentBone);
	}
	
	public void SendAnimationToPops(string clipName, Transform bone = null) {
//		Debug.Log("ArmatureTrigger[ " + name + " ]/SendAnimationToPops, clipName = " + clipName);
		_pops.PlayAnimation(clipName, bone);
	}
}
