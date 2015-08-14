using UnityEngine;
using System.Collections;

public class EventUnlockTrigger : LockableArmatureTrigger {
	
	public AnimationClip unlockClip;
	
	public string unlockEvent = "";
	public string unlockMessage = "";
	
	void Awake() {
		InitEventUnlockTrigger();
	}
	
	public void InitEventUnlockTrigger() {
		EventCenter.Instance.OnTriggerEvent += onUnlockEvent;
		Init();	
	}

	public void onUnlockEvent(string evt) {
		if(evt == unlockEvent) {
			IsLocked = false;
			IsEnabled = true;
			if(unlockClip != null) {
				SendAnimationToPops(unlockClip.name, _parentBone);
			}
			var eventCenter = EventCenter.Instance;
			string msg;
			if(unlockMessage != "") {
				msg = unlockMessage;
			} else {
				msg = name + " unlocked";
			}
			eventCenter.AddNote(msg);
			houseKeeping();
		}
	}
	
	public virtual void houseKeeping() {
		EventCenter.Instance.OnTriggerEvent -= onUnlockEvent;
	}
}
