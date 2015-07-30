using UnityEngine;
using System.Collections;

public class EventUnlockTrigger : LockableArmatureTrigger {
	
	public AnimationClip unlockClip;
	
	public string unlockEvent = "";
	public string unlockMessage = "";
	
	void Awake() {
		initEventUnlockTrigger();
	}
	
	public void initEventUnlockTrigger() {
		EventCenter.Instance.onTriggerEvent += onUnlockEvent;
		init ();	
	}

	public void onUnlockEvent(string evt) {
		Debug.Log("EventUnlockTrigger[ " + this.name + " ]/onUnlockEvent, evt = " + evt + ", unlockEvent = " + unlockEvent);
		if(evt == unlockEvent) {
			this.isLocked = false;
			this.isEnabled = true;
			if(unlockClip != null) {
				sendAnimationToPops(unlockClip.name, parentBone);
			}
			var eventCenter = EventCenter.Instance;
			string msg;
			if(unlockMessage != "") {
				msg = unlockMessage;
			} else {
				msg = this.name + " unlocked";
			}
			eventCenter.addNote(msg);
			houseKeeping();
			Debug.Log(" it is now unlocked: isLocked = " + this.isLocked + ", isEnabled = " + this.isEnabled);
		}
	}
	
	public virtual void houseKeeping() {
		EventCenter.Instance.onTriggerEvent -= onUnlockEvent;
	}
}
