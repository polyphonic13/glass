using UnityEngine;
using System.Collections;
using System;
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

	public int columns; 
	public int rows;

	private PuzzleWire _activeWire;

	private struct CellPosition {
		public int col;
		public int row; 
	}

	public void OnIntEvent(string type, int value) {
		Debug.Log ("DirectionalPuzzle/OnStringEvent, type = " + type + ", value = " + value);

		if(type == "toggle_wire_port" && !isSolved) {
			_onToggleWirePort (value);
		}
	}

	public override void Init() {
		int total = columns * rows; 

		_activeWire.port1 = -1;
		_activeWire.port2 = -1;

		base.Init ();

		EventCenter.Instance.OnIntEvent += OnIntEvent;
	}

	public override void Solve () {
		base.Solve ();
	}

	private void _onToggleWirePort(int port) {
		int index = _getWireIndexFromPort(port);
		if (index > -1) {
			_toggleWireActivatedByIndex (index, false);

			EventCenter.Instance.InvokeIntEvent ("port_plug_off", port);
		} else {
			if (_activeWire.port1 == -1) {
				_activeWire.port1 = port;

				EventCenter.Instance.InvokeIntEvent ("port_plug_on", port);
			} else {
				if (_getIsValidPuzzleWire (port)) {
					_activeWire.port2 = port;
					index = _getWireIndexFromPorts (_activeWire.port1, _activeWire.port2);
					if (index > -1) {
						_toggleWireActivatedByIndex (index, true);

						EventCenter.Instance.InvokeIntEvent ("port_plug_off", port);

						isSolved = _checkIsPuzzleSolved();
						Debug.Log ("DirectionalCircuitPuzzle[" + this.name + "].isSolved = " + isSolved);
						if (isSolved) {
							Solve ();
						}
					}
					_activeWire.port1 = -1;
					_activeWire.port2 = -1;
				} else {
					Debug.Log("non matching port2: " + port);
				}
			}
		}
	}

	private bool _getIsValidPuzzleWire(int port) {
		bool isAdjacent = false;

		CellPosition pos1 = _setCellPosition (_activeWire.port1);
		CellPosition pos2 = _setCellPosition (port);

		isAdjacent = _getIsAdjacent (pos1.row, pos2.row, pos1.col, pos2.col);

		if (!isAdjacent) {
			isAdjacent = _getIsAdjacent (pos1.col, pos2.col, pos1.row, pos2.row);
		}

		return isAdjacent;
	}

	private CellPosition _setCellPosition(int port) {
		CellPosition position = new CellPosition ();
		position.col = port % columns;
		position.row = port / rows;

		return position;
	}

	private bool _getIsAdjacent(int a, int b, int c, int d) {
		if (a != b) {
			return false;
		}

		if(d == (c + 1) || d == (c - 1)) {
			return true;
		}
		return false;
	}

	private int _getWireIndexFromPorts(int port1, int port2) {
		int index = -1;

		for (int i = 0, l = wires.Length; i < l; i++) {
			if ((wires [i].port1 == port1 && wires [i].port2 == port2) || (wires [i].port1 == port2 && wires [i].port2 == port1)) {
				index = i;
				break;
			}
		}

		return index;
	}

	private int _getWireIndexFromPort(int port) {
		int index = -1;

		for (int i = 0, l = wires.Length; i < l; i++) {
			if (wires[i].isActivated && (wires [i].port1 == port || wires [i].port2 == port)) {
				index = i;
				break;
			}
		}
		return index;
	}

	private void _toggleWireActivatedByIndex(int index, bool isActivated) {
		wires [index].isActivated = isActivated;
		wireColliders [index].SetActive (!isActivated);

		ToggleChildActive (children [index], isActivated);
	}

	private bool _checkIsPuzzleSolved() {
		int count = 0;
		int l = solution.Length;

		for (int i = 0; i < l; i++) {
			if (wires [solution [i]].isActivated) {
				count++;
			}
		}

		if (count != l) {
			return false;
		}
		return true; 
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
	public int port1;
	public int port2;
	public bool isActivated;
}
