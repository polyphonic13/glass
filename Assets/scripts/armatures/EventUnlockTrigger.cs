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
		Debug.Log("EventUnlockTrigger[ " + name + " ]/onUnlockEvent, evt = " + evt + ", unlockEvent = " + unlockEvent);
		if(evt == unlockEvent) {
			IsLocked = false;
			_isEnabled = true;
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
			Debug.Log(" it is now unlocked: IsLocked = " + IsLocked + ", _isEnabled = " + _isEnabled);
		}
	}
	
	public virtual void houseKeeping() {
		EventCenter.Instance.OnTriggerEvent -= onUnlockEvent;
	}
}
