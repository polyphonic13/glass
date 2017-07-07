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

	public int cols;
	public int rows;
	public int vRows;

	public int[] solution; 

	public string wiresPath = "";
	public string wireHighlightsPath = ""; 
	public string wireCollidersPath = ""; 

	private List<PuzzleWire> _wireChildren;
	List<List<int>> _ports;

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
		_initPorts ();
		_initPuzzleWires ();
		EventCenter.Instance.OnIntEvent += OnIntEvent;
	}

	public override void Solve () {
		base.Solve ();
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
				puzzleWire.isSolutionWire = _getIsInSolutionArray (count);
				puzzleWire.siblings = _getSiblingsFromPorts(count);
				Debug.Log (" adding puzzle wire [" + t.name + "] to list");
				_wireChildren.Add (puzzleWire);

				count++;
			}
			Debug.Log ("_wireChildren count = " + _wireChildren.Count);
		}
	}

	private List<int> _getSiblingsFromPorts(int index) {
		Debug.Log ("_getSiblingsFromPorts for: " + index);
		List<int> siblings = new List<int>();
		foreach(List<int> port in _ports) {
			if (port.Contains (index)) {
				foreach (int wire in port) {
					if (wire != index) {
						siblings.Add (wire);
						Debug.Log ("  added: " + wire);
					}
				}
			}
		}
		return siblings;
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

		if (isInserted) {
			foreach (int s in wire.siblings) {
				Debug.Log ("s = " + s);
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
	public List<int> siblings;
}

/*
// 4x4
var totalWires = 24;
var rows = 4;
var cols = 4;
var vRows = 3;

// 5x5
// var totalWires = 40;
// var rows = 5;
// var cols = 5;
// var vRows = 4;

var total = rows * cols;
var half = Math.ceil(total / 2);
var i;
var row = 0; 
var col = 0;

var ports = [];
var wires = [];
var siblings = [];

for(i = 0; i < totalWires; i++) {
	wires.push([]);
}

for(i = 0; i < total; i++) {
	var adjacent = [];

	if(i > 0 && i % rows === 0) {
		row++;
		col = 0;
	}

	// horizontal
	if(col === 0) {
		// first col
		adjacent.push(i - row);

	} else if(col === cols - 1) {
		// last col
		adjacent.push(i - (row + 1));
	} else {
		// middle cols
		adjacent.push(i - (row + 1));
		adjacent.push(i - row);
	}

	if(row === 0) {
		// first row
		adjacent.push(i + (vRows * rows));
	} else if(row === rows - 1) {
		// last row
		adjacent.push(i + (vRows * rows) - rows);
	} else {
		adjacent.push(i + (vRows * rows) - rows);
		adjacent.push(i + (vRows * rows));

	}
	console.log('adjacent[' + i + '] = ' + JSON.stringify(adjacent));
	col++;

	_addPortsToPuzzleWire(adjacent, i);

	ports.push(adjacent);

}

_assignSiblings();

function _addPortsToPuzzleWire(adjacent, port) {
	adjacent.forEach(function(wire) {
		if(wires[wire].indexOf(port) === -1) {
			wires[wire].push(port);
		}
	});
}

function _assignSiblings() {
	wires.forEach(function(wire, index) {
		var sibling = [];
		var portsA = ports[wire[0]];
		var portsB = ports[wire[1]];

		portsA.forEach(function(port) {
			if(port !== index) {
				sibling.push(port);

			}
		});
		portsB.forEach(function(port) {
			if(port !== index && sibling.indexOf(port) === -1) {
				sibling.push(port);

			}
		});
		siblings.push(sibling);
	});
}

function _insertWire(index) {
	console.log('_insertWire ' + index + ', remove = ' + JSON.stringify(siblings[index]));
}
*/