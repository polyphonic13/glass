using UnityEngine;
using System.Collections;

public class ContainerUnlockTrigger : EventUnlockTrigger
{

	void Awake() {
		initContainerUnlockTrigger();
	}
	
	void initContainerUnlockTrigger() {
		EventCenter.Instance.onCollectedEvent += this.onUnlockEvent;
		init ();
	}
	
	public override void houseKeeping() {
		EventCenter.Instance.onCollectedEvent -= this.onUnlockEvent;
	}
}

