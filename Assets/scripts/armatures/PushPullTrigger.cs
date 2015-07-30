using UnityEngine;
using System.Collections;

public class PushPullTrigger : LockableArmatureTrigger {

	void Awake() {
		initLockableArmatureTrigger(MouseManager.Instance.PUSH_CURSOR);
	}
	
}
