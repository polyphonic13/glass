using UnityEngine;
using System.Collections;

public class PushPullTrigger : LockableArmatureTrigger {

	void Awake() {
		InitLockableArmatureTrigger(MouseManager.Instance.PUSH_CURSOR);
	}
	
}
