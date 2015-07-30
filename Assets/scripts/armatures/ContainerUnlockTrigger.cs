using UnityEngine;
using System.Collections;

public class ContainerUnlockTrigger : EventUnlockTrigger
{

	void Awake() {
		InitContainerUnlockTrigger();
	}
	
	void InitContainerUnlockTrigger() {
		EventCenter.Instance.onCollectedEvent += onUnlockEvent;
		Init();
	}
	
	public override void houseKeeping() {
		EventCenter.Instance.onCollectedEvent -= onUnlockEvent;
	}
}

