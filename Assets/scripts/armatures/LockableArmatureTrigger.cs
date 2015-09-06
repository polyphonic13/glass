public class LockableArmatureTrigger : OpenCloseArmatureTrigger {

	public string _lockedItemName = ""; 
	public string _keyName = "";
	
	public bool IsLocked { get; set; }

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
			 if(Inventory.Instance.HasItem(_keyName)) {
				IsLocked = false;
				HandleOpenClose();
			 } else {
				 EventCenter.Instance.AddNote(_lockedItemName + " is locked");
			 }
		}
	}

}

