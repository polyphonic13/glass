using UnityEngine;
using System.Collections;
using Polyworks;

public class Puzzle : MonoBehaviour {
	public const string UNLOCK_EVENT = "unlock_puzzle";
	public const string ACTIVATE_EVENT = "activate_puzzle";

	public string activateValue; 
	public GameObject mainCollider; 

	public PuzzleComponent[] puzzleComponents;
	public Transform[] hiddenChildren;

	private bool _isActive = false; 

	public void OnStringEvent(string type, string value) {
		Debug.Log ("Puzzle[" + this.name + "]/OnStringEvent, type = " + type + ", value = " + value);
		if (type == Puzzle.ACTIVATE_EVENT && value == activateValue) {
			_toggleActive (true);
			EventCenter.Instance.ChangeContext(InputContext.PUZZLE, this.name);
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
		Debug.Log ("Puzzle[" + this.name + "]/_toggleActive, isActivated = " + isActivated);
		_isActive = isActivated;
		mainCollider.SetActive (!_isActive);
	}
}
