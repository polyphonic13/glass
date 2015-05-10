﻿using UnityEngine;
using System.Collections;

public class SceneChanger : MonoBehaviour {

	public string targetScene;
	public int targetRoom;

	void OnTriggerEnter(Collider tgt) {
		Debug.Log("scene changer trigger, tgt.tag = " + tgt.gameObject.tag);
		if(tgt.gameObject.tag == "Player") {
			GameControl.instance.changeScene(targetScene, targetRoom);
		}
	}

}
