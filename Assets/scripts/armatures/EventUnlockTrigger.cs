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
			Debug.Log (this.name + "/onUnlockEvent, evt = " + evt + ", unlockClip = " + unlockClip.name + ", IsLocked = " + IsLocked);
			IsLocked = false;
			IsEnabled = true;
			if(unlockClip != null) {
				Debug.Log("sending clip to pops");
				SendAnimationToPops(unlockClip.name, _parentBone);
			}

			string msg;
			if(unlockMessage != "") {
				msg = unlockMessage;
			} else {
				msg = name + " unlocked";
			}
			Debug.Log("going to add this msg: " + msg);
			EventCenter.Instance.AddNote(msg);
			houseKeeping();
		}
	}
	
	public virtual void houseKeeping() {
		EventCenter.Instance.OnTriggerEvent -= onUnlockEvent;
	}
}
