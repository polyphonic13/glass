using UnityEngine;
using System.Collections;

public class UnderWaterTrigger : MonoBehaviour {

	[SerializeField] bool _underWater; 

	void OnTriggerEnter(Collider tgt) {
//		Debug.Log("under water trigger, tgt.tag = " + tgt.gameObject.tag);
		if(tgt.gameObject.tag == "Player") {
			EventCenter.Instance.changeUnderWater(_underWater);
		}
	}

}
