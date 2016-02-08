using UnityEngine;
using System.Collections;

public class LockableArmatureTrigger : OpenCloseArmatureTrigger {

	public string _keyName = "";

	public bool IsLocked = true;
//	public bool IsLocked { get; set; }

	public void Awake() {
		InitLockableArmatureTrigger();
	}
	
	public void InitLockableArmatureTrigger() {
		InitOpenCloseArmatureTrigger();
		Init();
	}

	public override void HandleAnimation() {
		HandleLockCheck();
	}
	
	public void HandleLockCheck() {
//		Debug.Log("LockableArmatureTrigger[ " + name + " ]/HandleLockCheck, IsLocked = " + IsLocked);
		if(!IsLocked) {
			HandleOpenClose();
		} else {
//			 if(Inventory.Instance.HasItem(_keyName)) {
//				_unlock ();
//			 } else {
				 EventCenter.Instance.AddNote("The " + this.ItemName + " is locked");
//			 }
		}
	}

	public void Unlock() {
		_unlock ();
	}

	private void _unlock() {
		IsLocked = false;
		EventCenter.Instance.AddNote("The " + this.ItemName + " was unlocked");
		HandleOpenClose();
	}
}

