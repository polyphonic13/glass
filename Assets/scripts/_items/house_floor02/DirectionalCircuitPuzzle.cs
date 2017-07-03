using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Polyworks; 

/*
 *  0 | 1
 * -------
 *  2 | 3
 */

public class DirectionalCircuitPuzzle : Puzzle
{
	public PuzzleWire[] wires;
	public GameObject[] wireColliders; 

	public int[] solution; 

	public string wiresPath = "";
	public string wireHighlightsPath = ""; 
	public string wireCollidersPath = ""; 

	private List<PuzzleWire> _wireChildren;
	
	public void OnIntEvent(string type, int value) {
		Debug.Log ("DirectionalPuzzle/OnIntEvent, type = " + type + ", value = " + value);

		switch (type) {
		case "insert_wire":
			_toggleWireInserted (value, true);
			break;

		case "remove_wire":
			_toggleWireInserted (value, false);
			break;
		}
	}

	public override void Init() {
		base.Init ();
		_initPuzzleWires ();
		EventCenter.Instance.OnIntEvent += OnIntEvent;
	}

	public override void Solve () {
		base.Solve ();
	}

	private void _initPuzzleWires() {
		_wireChildren = new List<PuzzleWire> ();

		if (wiresPath != "") {
			Transform wireHolder = transform.Find (wiresPath);
			int count = 0; 

			foreach(Transform t in wireHolder) {
				PuzzleWire puzzleWire = new PuzzleWire ();
				puzzleWire.gameObject = t.gameObject;
				puzzleWire.index = count;
				puzzleWire.isActivated = false;
				puzzleWire.gameObject.SetActive (false);
				puzzleWire.isSolutionWire = _getIsInSolutionArray (count);

				Debug.Log (" adding puzzle wire [" + t.name + "] to list");
				_wireChildren.Add (puzzleWire);

				count++;
			}
			Debug.Log ("_wireChildren count = " + _wireChildren.Count);
		}
	}

	private bool _getIsInSolutionArray(int count) {
		for (int i = 0; i < solution.Length; i++) {
			if (solution [i] == count) {
				return true;
			}
		}
		return false;
	}

	private void _toggleWireInserted(int index, bool isInserted) {
		if (index >= _wireChildren.Count) {
			return;
		}
		int childIndex = _getWireByIndex (index, _wireChildren);

		if (childIndex == -1) {
			return;
		}
		PuzzleWire wire = _wireChildren [childIndex];
		wire.gameObject.SetActive (isInserted);
		wire.isActivated = isInserted;
		_wireChildren [childIndex] = wire;

		_checkIsSolved ();
	}

	private void _checkIsSolved() {
		isSolved = true;
		for (int i = 0; i < solution.Length; i++) {
			Debug.Log ("solution[" + i + "] = " + solution [i] + " is activated = " + _wireChildren [solution [i]].isActivated);
			if(!_wireChildren[solution[i]].isActivated) {
				isSolved = false;
			}
		}
		if (isSolved) {
			base.Solve ();
		}
		Debug.Log ("DirectionalCircuitPuzzle[" + this.name + "]/_checkIsSolved, isSolved = " + isSolved);
	}

	private int _getWireByIndex(int index, List<PuzzleWire> list) {
		for (int i = 0; i < list.Count; i++) {
			if (list [i].index == index) {
				return i;
			}
		}
		return -1;
	}

	private void _initWireColliders() {

	}

	private void OnDestroy() {
		EventCenter ec = EventCenter.Instance; 
		if(ec != null) {
			ec.OnIntEvent -= OnIntEvent;
		}
	}
}

[Serializable]
public struct PuzzleWire {
	public int index;
	public GameObject gameObject;
	public bool isActivated;
	public bool isSolutionWire;
}
