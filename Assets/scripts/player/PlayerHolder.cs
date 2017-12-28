using UnityEngine;
using System.Collections;

public class PlayerHolder : MonoBehaviour {
	public Transform holder;

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Player") {
			// Debug.Log ("parenting player to " + holder.transform);
			col.transform.parent = holder.transform;
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.gameObject.tag == "Player") {
			// Debug.Log ("removing player parenting");
			col.transform.parent = null;
		}

	}
}
