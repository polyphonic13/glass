using UnityEngine;
using System.Collections;
using Polyworks;

public class Puzzle : MonoBehaviour {

	public Transform[] hiddenChildren; 

	public virtual void Init() {
		GameObjectUtils.DeactivateFromTransforms (hiddenChildren);
	}

	private void Awake() {
		Init ();
	}
}
