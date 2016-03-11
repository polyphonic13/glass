using UnityEngine;
using System.Collections;

public class PlayerHolder : MonoBehaviour {
	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Player") {
			col.transform.parent = this.transform.parent;
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.gameObject.tag == "Player") {
			col.transform.parent = null;
		}

	}
}
