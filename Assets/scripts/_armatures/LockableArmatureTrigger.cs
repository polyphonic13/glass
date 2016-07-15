using UnityEngine;
using System.Collections;
using Polyworks; 

public class LockableArmatureTrigger : OpenCloseArmatureTrigger {

	public string _keyName = "";

	public bool IsLocked = true;
//	public bool IsLocked { get; set; }

	public void Awake() {
		InitLockableArmatureTrigger();
	}
	
	public void InitLockableArmatureTrigger() {
		InitOpenCloseArmatureTrigger();
	}

	public override void HandleAnimation() {
		HandleLockCheck();
	}
	
	public void HandleLockCheck() {
//		// Debug.Log("LockableArmatureTrigger[ " + name + " ]/HandleLockCheck, IsLocked = " + IsLocked);
		if(!IsLocked) {
			HandleOpenClose();
		} else {
			EventCenter.Instance.AddNote("The " + this.data.itemName + " is locked");
		}
	}

	public void Unlock() {
		_unlock ();
	}

	private void _unlock() {
		IsLocked = false;
		EventCenter.Instance.AddNote("The " + this.data.itemName + " was unlocked");
		HandleOpenClose();
	}
}

