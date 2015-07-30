using UnityEngine;
using System.Collections;

public class EventAnimationParent : ArmatureParent {
	
	public AnimationClip animationClip;
	public string eventName = "";
	
	void Awake() {
		initEventAnimationParent();
	}
	
	public void initEventAnimationParent() {
		if(eventName != "") {
			EventCenter.Instance.onTriggerEvent += this.onTriggerEvent;
		}
		init();
	}

	public void onTriggerEvent(string evt) {
		Debug.Log("EventAnimationParent[ " + this.name + " ]/onTriggerEvent, evt = " + evt + ", eventName = " + eventName + ", animationClip = " + animationClip.name);
		if(evt == eventName && animationClip != null) {
			playAnimation(animationClip.name);
		}
	}
}
