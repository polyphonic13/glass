using UnityEngine;
using System.Collections;

public class EventUnlockTrigger : LockableArmatureTrigger {
	
	public AnimationClip unlockClip;

	public bool isUnlockEventOpens = false;
	public string unlockEvent = "";
	public string unlockMessage = "";
	
	void Awake() {
		InitEventUnlockTrigger();
	}
	
	public void InitEventUnlockTrigger() {
		EventCenter.Instance.OnTriggerEvent += onUnlockEvent;
		Init();	
		isEnabled = false;
	}

	public void onUnlockEvent(string evt) {
		if(evt == unlockEvent && IsLocked) {
			IsLocked = false;
			isEnabled = true;
			if(unlockClip != null) {
				SendAnimationToPops(unlockClip.name, _parentBone);
				if(isUnlockEventOpens) {
					IsOpen = true;
				}
			}

			string msg;
			if(unlockMessage != "") {
				msg = unlockMessage;
				EventCenter.Instance.AddNote(msg);
			}
			houseKeeping();
		}
	}
	
	public virtual void houseKeeping() {
		EventCenter.Instance.OnTriggerEvent -= onUnlockEvent;
	}
}
