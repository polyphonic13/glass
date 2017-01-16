using UnityEngine;
using System.Collections;
using Polyworks;

public class Puzzle : MonoBehaviour {
	public const string UNLOCK_EVENT = "unlock_puzzle";
	public const string ACTIVATE_EVENT = "activate_puzzle";

	public string activateValue; 

	public Transform[] hiddenChildren;

	public void OnStringEvent(string type, string value) {
		if (type == Puzzle.ACTIVATE_EVENT && value == activateValue) {
			Debug.Log ("Puzzle["+this.name+"]/OnStringEvent, type = " + type + ", value = " + value);
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
