using UnityEngine;
using System.Collections;
using Polyworks;

public class Puzzle : MonoBehaviour {
	public const string UNLOCK_EVENT = "unlock_puzzle";
	public const string ACTIVATE_EVENT = "activate_puzzle";

	public string activateValue; 
	public GameObject mainCollider; 

	public Transform[] hiddenChildren;

	private bool _isActive = false; 

	public void OnStringEvent(string type, string value) {
		if (type == Puzzle.ACTIVATE_EVENT && value == activateValue) {
			Debug.Log ("Puzzle[" + this.name + "]/OnStringEvent, type = " + type + ", value = " + value);
			_toggleActive (true);
		} else if(_isActive) {
			_toggleActive (false);
		}
	}

	public virtual void Init() {
		if (hiddenChildren.Length > 0) {
			GameObjectUtils.DeactivateFromTransforms (hiddenChildren);
		}

		EventCenter ec = EventCenter.Instance;
		ec.OnStringEvent += this.OnStringEvent;

		_toggleActive (false);

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

	private void _toggleActive(bool isActivated) {
		_isActive = isActivated;
		mainCollider.SetActive (!_isActive);
	}
}
