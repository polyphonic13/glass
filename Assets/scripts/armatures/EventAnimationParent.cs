using UnityEngine;
using System.Collections;

public class EventAnimationParent : ArmatureParent {
	
	public AnimationClip animationClip;
	public string eventName = "";
	
	void Awake() {
		InitEventAnimationParent();
	}
	
	public void InitEventAnimationParent() {
		if(eventName != "") {
			EventCenter.Instance.onTriggerEvent += onTriggerEvent;
		}
		Init();
	}

	public void onTriggerEvent(string evt) {
		Debug.Log("EventAnimationParent[ " + name + " ]/onTriggerEvent, evt = " + evt + ", eventName = " + eventName + ", animationClip = " + animationClip.name);
		if(evt == eventName && animationClip != null) {
			playAnimation(animationClip.name);
		}
	}
}
