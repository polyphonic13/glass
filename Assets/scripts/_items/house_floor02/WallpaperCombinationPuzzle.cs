using UnityEngine;
using System.Collections;
using Polyworks; 
using System;
using System.Collections.Generic;

public class WallpaperCombinationPuzzle : Puzzle
{
	public CombinationDial[] dials; 

	private List<bool> _correctPositions;

	public void OnIntEvent(string type, int value) {
		Debug.Log ("WallpaperCombinationPuzzle/OnIntEvent, type = " + type + ", value = " + value);
		for (int i = 0; i < dials.Length; i++) {
			if (dials [i].name == type) {
				if (dials [i].rotation == value) {
					_correctPositions [i] = true;

					isSolved = _checkSolved ();
					Debug.Log (" isSolved = " + isSolved);
					if (isSolved) {
						Solve ();
					}
				} else {
					_correctPositions [i] = false;
				}
				break;
			}
		}
	}

	public override void Activate() {
		base.Activate ();
		if (!isSolved) {
			EventCenter.Instance.OnIntEvent += OnIntEvent;
		}
	}

	public override void Deactivate () {
		base.Deactivate ();
		EventCenter.Instance.OnIntEvent -= OnIntEvent;
	}

	public override void Solve () {
		base.Solve ();
		EventCenter.Instance.OnIntEvent -= OnIntEvent;
	}

	private void Awake() {
		_correctPositions = new List<bool> ();
		for (int i = 0; i < dials.Length; i++) {
			_correctPositions.Add (false);
		}
		Debug.Log ("_correctPositions.Count = " + _correctPositions.Count);
	}

	private bool _checkSolved() {
		for (int i = 0; i < _correctPositions.Count; i++) {
			if (_correctPositions [i] == false) {
				return false;
			}
		}
		return true;
	}

	private void OnDestroy() {
		EventCenter ec = EventCenter.Instance;
		if (ec != null) {
			ec.OnIntEvent -= OnIntEvent;
		}
	}
}

[Serializable]
public struct CombinationDial {
	public string name;
	public int rotation;
}

