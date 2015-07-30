using UnityEngine;
using System.Collections;

public class LeftHand : MonoBehaviour {

	public OVRCamera parentCam;

	// Use this for initialization
	void Start () {
		if(parentCam != null) {
			this.gameObject.transform.parent = parentCam.transform;
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
