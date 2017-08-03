using UnityEngine;
using System.Collections;
using System;
using Polyworks;

public class Puzzle : MonoBehaviour {
	public const string UNLOCK_EVENT = "unlock_puzzle";
	public const string ACTIVATE_EVENT = "activate_puzzle";
	public const string SOLVED_EVENT = "puzzle_solved";

	public string activateValue; 
	public GameObject mainCollider; 

	public PuzzleChild[] children;

	public bool isSolved = false; 

	private bool _isActive = false; 

	#region eventhandlers
	public void OnStringEvent(string type, string value) {
		Debug.Log ("Puzzle[" + this.name + "]/OnStringEvent, type = " + type + ", value = " + value);
		if (type == Puzzle.ACTIVATE_EVENT) {
			if (value == activateValue) {
				Activate ();
			} else if (_isActive) {
				Deactivate ();
			}
		}
	}

	public void OnChangeContext(InputContext context) {
		if (context != InputContext.PUZZLE) {
			_toggleActive (false);
		}
	}
	#endregion

	#region public methods
	public virtual void Init() {
		InitChildren();
//		EventCenter ec = EventCenter.Instance;
//		ec.OnStringEvent += this.OnStringEvent;

		_toggleActive (false);

	}

	public virtual void Enable() {
		Debug.Log ("Puzzle[" + this.name + "]/Enable");
		EventCenter ec = EventCenter.Instance;
		ec.OnStringEvent += this.OnStringEvent;
	}

	public virtual void Disable() {
		EventCenter ec = EventCenter.Instance;
		if (ec != null) {
			ec.OnStringEvent -= this.OnStringEvent;
		}
	}

	public virtual void InitChildren() {
		
		for (int i = 0, l = children.Length; i < l; i++) {
			Item item = children [i].gameObject.GetComponent<Item> ();
			if (item != null) {
				children [i].item = item;
			}

			if (children [i].isDeactivatedOnInit) {
				ToggleChildActive (children [i], false);
			}
		}
	}

	public virtual void ToggleChildActive(PuzzleChild child, bool isActivated) {
		child.isActive = isActivated;
		child.gameObject.SetActive (isActivated);

		if (child.item != null) {
			child.item.isEnabled = isActivated;
		}
	}

	public virtual void Activate() {
		_toggleActive (true);
		EventCenter.Instance.ChangeContext (InputContext.PUZZLE, this.name);
	}

	public virtual void Deactivate() {
		_toggleActive(false);
	}

	public virtual void Solve() {
		_toggleChildrenOnSolved ();
		EventCenter.Instance.InvokeStringEvent (Puzzle.SOLVED_EVENT, this.name);
	} 
	#endregion

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
//		Debug.Log ("Puzzle[" + this.name + "]/_toggleActive, isActivated = " + isActivated);
		_isActive = isActivated;
		mainCollider.SetActive (!_isActive);
	}

	private void _toggleChildrenOnSolved() {
		for (int i = 0, l = children.Length; i < l; i++) {
			if (children [i].isActivatedOnSolved) {
				ToggleChildActive (children [i], true);
			} else if (children [i].isDeactivatedOnSolved) {
				ToggleChildActive (children [i], false);
			}
		}
	}
}

[Serializable]
public struct PuzzleChild {
	public GameObject gameObject;
	public Item item;
	public bool isActive;
	public bool isDeactivatedOnInit;
	public bool isActivatedOnSolved;
	public bool isDeactivatedOnSolved;
}