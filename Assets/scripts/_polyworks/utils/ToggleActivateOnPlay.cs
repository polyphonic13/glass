using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ActivateTarget {
	public GameObject gameObject;
	public bool isActivateOnPlay;
}

public class ToggleActivateOnPlay : MonoBehaviour {

	public ActivateTarget[] targets;

	private void Awake() {
		for (int i = 0; i < targets.Length; i++) 
		{
			targets[i].gameObject.SetActive(targets[i].isActivateOnPlay);
		}
	}

}
