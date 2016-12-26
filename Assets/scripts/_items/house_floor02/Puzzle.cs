using UnityEngine;
using System.Collections;
using Polyworks;

public class Puzzle : MonoBehaviour {
	public const string UNLOCK_EVENT = "unlock_puzzle";
	public const string ACTIVATE_EVENT = "activate_puzzle";

	public Transform[] hiddenChildren; 

	public void OnStringEvent(string type, string value) {
		string expectedValue = this.name + "_collider";
		if (type == Puzzle.ACTIVATE_EVENT) {
			Debug.Log ("Puzzle["+this.name+"]/OnStringEvent, type = " + type + ", value = " + value + ", expectedValue = " + expectedValue);

		}
	}

	public virtual void Init() {
		GameObjectUtils.DeactivateFromTransforms (hiddenChildren);

		EventCenter ec = EventCenter.Instance;
		ec.OnStringEvent += this.OnStringEvent;
	}

	private void Awake() {
		Init ();
	}

	private void OnDestroy() {
		EventCenter ec = EventCenter.Instance;
		if (ec != null) {
			ec.OnStringEvent -= this.OnStringEvent;
		}
	}
}
