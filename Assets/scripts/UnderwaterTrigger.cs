using UnityEngine;
using System.Collections;

public class UnderwaterTrigger : MonoBehaviour {

	[SerializeField] bool _isOnWater; 

	void OnTriggerEnter(Collider tgt) {
//		Debug.Log("under water trigger, tgt.tag = " + tgt.gameObject.tag);
		if(tgt.gameObject.tag == "Player") {
			EventCenter.Instance.changeOnWater(_isOnWater, this.transform);
		}
	}

}
