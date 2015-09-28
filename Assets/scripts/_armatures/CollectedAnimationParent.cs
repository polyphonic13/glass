using UnityEngine;
using System.Collections;

public class CollectedAnimationParent : EventAnimationParent {

	void Awake() {
		InitEventAnimationParent();
	}
	
	public void InitEventAnimationParent() {
		Debug.Log("CollectedAnimationParent[ " + name + " ]/InitEventAnimationParent");
		if(eventName != "") {
			EventCenter.Instance.OnTriggerCollectedEvent += OnTriggerEvent;
		}
		Init();
	}

}

