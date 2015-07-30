using UnityEngine;
using System.Collections;

public class CollectedAnimationParent : EventAnimationParent {

	void Awake() {
		initEventAnimationParent();
	}
	
	public void initEventAnimationParent() {
		Debug.Log("CollectedAnimationParent[ " + this.name + " ]/initEventAnimationParent");
		if(eventName != "") {
			EventCenter.Instance.onCollectedEvent += this.onTriggerEvent;
		}
		init();
	}

}

