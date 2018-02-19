using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Polyworks; 

public class DirectionalCircuitPuzzle : Puzzle
{
	public int cols;
	public int rows;
	public int vRows;

	public int[] solution; 

	public string wiresPath = "";

	public string wiresPrefabPath = ""; 

	public string solvedSwitchEventValue = ""; 
	public string solvedSwitchEventType = "solvedSwitchThrown";

	private List<PuzzleWire> _wireChildren;
	List<List<int>> _ports;

	private bool _isSwitchThrown = false; 

	public void OnIntEvent(string type, int value) {
		Log ("DirectionalPuzzle/OnIntEvent, type = " + type + ", value = " + value);

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
		_initPorts ();
		_initPuzzleWires ();
	}

	public override void Activate() {
		base.Activate ();
		if (!isCompleted) {
			EventCenter.Instance.OnIntEvent += OnIntEvent;
		}
	}

	public override void Deactivate () {
		base.Deactivate ();

		if (!isCompleted) {
			_removeAllWires ();

			if (wiresPrefabPath != "") {
				Inventory inventory = Game.Instance.GetPlayerInventory ();
				inventory.AddFromPrefabPath (wiresPrefabPath);
			}
		}
		_removeListeners();
	}

	public override void Solve () {
		base.Solve ();
		_removeListeners();
	}

	private void _initPorts() {
		_ports = new List<List<int>> ();

		int total = rows * cols; 
		int row = 0;
		int col = 0;

		List<int> wires; 

		for (int i = 0; i < total; i++) {
			wires = new List<int> ();

			if(i > 0 && i % rows == 0) {
				row++;
				col = 0;
			}

			// horizontal
			if(col == 0) {
				// first col
				wires.Add(i - row);

			} else if(col == cols - 1) {
				// last col
				wires.Add(i - (row + 1));
			} else {
				// middle cols
				wires.Add(i - (row + 1));
				wires.Add(i - row);
			}

			if(row == 0) {
				// first row
				wires.Add(i + (vRows * rows));
			} else if(row == rows - 1) {
				// last row
				wires.Add(i + (vRows * rows) - rows);
			} else {
				wires.Add(i + (vRows * rows) - rows);
				wires.Add(i + (vRows * rows));

			}
			col++;

			_ports.Add(wires);
		}
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
				puzzleWire.siblings = _getSiblingsFromPorts(count);
				_wireChildren.Add (puzzleWire);

				count++;
			}
		}
	}

	private List<int> _getSiblingsFromPorts(int index) {
		List<int> siblings = new List<int>();
		foreach(List<int> port in _ports) {
			if (port.Contains (index)) {
				foreach (int wire in port) {
					if (wire != index) {
						siblings.Add (wire);
					}
				}
			}
		}
		return siblings;
	}

	private void _removeAllWires() {
		for (int i = 0; i < _wireChildren.Count; i++) {
			_toggleWireInserted (i, false);
		}
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

		if (isInserted) {
			foreach (int s in wire.siblings) {
				PuzzleWire sibling = _wireChildren[s];
				sibling.isActivated = false;
				sibling.gameObject.SetActive (false);
				_wireChildren [s] = sibling;
			}
		}

		wire.gameObject.SetActive (isInserted);
		wire.isActivated = isInserted;
		_wireChildren [childIndex] = wire;

		_checkIsSolved ();
	}

	private void _checkIsSolved() {
		isSolved = true;
		for (int i = 0; i < solution.Length; i++) {
//			Log ("solution[" + i + "] = " + solution [i] + " is activated = " + _wireChildren [solution [i]].isActivated);
			if(!_wireChildren[solution[i]].isActivated) {
				isSolved = false;
			}
		}
		if (isSolved) {
			Solve ();
		}
		// Log ("DirectionalCircuitPuzzle[" + this.name + "]/_checkIsSolved, isSolved = " + isSolved);
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

	private void _removeListeners() {
		EventCenter ec = EventCenter.Instance; 
		if(ec != null) {
			ec.OnIntEvent -= OnIntEvent;
		}
	}

	private void OnDestroy() {
		_removeListeners();
	}
}

[Serializable]
public struct PuzzleWire {
	public int index;
	public GameObject gameObject;
	public bool isActivated;
	public List<int> siblings;
}
