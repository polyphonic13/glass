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
		EventCenter.Instance.onTriggerEvent += onUnlockEvent;
		Init();	
	}

	public void onUnlockEvent(string evt) {
		Debug.Log("EventUnlockTrigger[ " + name + " ]/onUnlockEvent, evt = " + evt + ", unlockEvent = " + unlockEvent);
		if(evt == unlockEvent) {
			isLocked = false;
			isEnabled = true;
			if(unlockClip != null) {
				sendAnimationToPops(unlockClip.name, parentBone);
			}
			var eventCenter = EventCenter.Instance;
			string msg;
			if(unlockMessage != "") {
				msg = unlockMessage;
			} else {
				msg = name + " unlocked";
			}
			eventCenter.addNote(msg);
			houseKeeping();
			Debug.Log(" it is now unlocked: isLocked = " + isLocked + ", isEnabled = " + isEnabled);
		}
	}
	
	public virtual void houseKeeping() {
		EventCenter.Instance.onTriggerEvent -= onUnlockEvent;
	}
}
