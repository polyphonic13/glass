using UnityEngine;
using System.Collections;

public class ActivateOnPlay : MonoBehaviour {

	public GameObject[] objects; 

	void Awake () {
		for (int i = 0; i < objects.Length; i++) {
			objects [i].SetActive (true);
		}
	}
}
