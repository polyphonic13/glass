public class LockableArmatureTrigger : OpenCloseArmatureTrigger {

	public string _lockedItemName = ""; 
	public string _keyName = "";
	
	public bool IsLocked { get; set; }

	public void Awake() {
		InitLockableArmatureTrigger();
	}
	
	public void InitLockableArmatureTrigger(int activeCursor = 1) {
		InitOpenCloseArmatureTrigger();
		Init(activeCursor);
	}

	public override void HandleAnimation() {
		HandleLockCheck();
	}
	
	public void HandleLockCheck() {
//		Debug.Log("LockableArmatureTrigger[ " + this.name + " ]/HandleLockCheck, IsLocked = " + IsLocked);
		if(!IsLocked) {
			HandleOpenClose();
		} else {
			// if(_player.inventory.hasItem(_keyName)) {
				// string itemName = _player.inventory.getItemName(_keyName);
				// EventCenter.Instance.AddNote(_lockedItemName + " unlocked with " + itemName);
				IsLocked = false;
				HandleOpenClose();
			// } else {
				// EventCenter.Instance.AddNote("The " + _lockedItemName + " is locked");
			// }
		}
	}

}

