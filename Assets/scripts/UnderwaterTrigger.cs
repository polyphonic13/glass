using UnityEngine;
using System.Collections;

public class UnderwaterTrigger : MonoBehaviour {

	public bool isOnWater; 

	void OnTriggerEnter(Collider tgt) {
//		Debug.Log(this.name + " under water trigger, tgt.tag = " + tgt.gameObject.tag + ", isOnWater = " + isOnWater);
		if(tgt.gameObject.tag == "Player") {
			EventCenter.Instance.changeOnWater(isOnWater, this.transform);
		}
	}

}
