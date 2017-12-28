using UnityEngine;
using System.Collections;

public class ContainerUnlockTrigger : EventUnlockTrigger
{

	void Awake() {
		InitContainerUnlockTrigger();
	}
	
	void InitContainerUnlockTrigger() {
//		EventCenter.Instance.OnTriggerCollectedEvent += onUnlockEvent;
	}
	
	public override void houseKeeping() {
//		EventCenter.Instance.OnTriggerCollectedEvent -= onUnlockEvent;
	}
}

