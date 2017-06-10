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
	public PortCombination[] wires; 
	public int[] solution; 

	public int columns; 
	public int rows;

	private PortCombination _activePort;

	private struct CellPosition {
		public int col;
		public int row; 
	}

	public void OnIntEvent(string type, int value) {
		Debug.Log ("DirectionalPuzzle/OnStringEvent, type = " + type + ", value = " + value);

		if(type == "wire_state_change") {
			_onWireStateChanged (value);
		}
	}

	public override void Init() {
		int total = columns * rows; 

		for (int i = 0; i < children.Length; i++) {
			
		}

		_activePort.port1 = -1;
		_activePort.port2 = -1;

		base.Init ();

		EventCenter.Instance.OnIntEvent += OnIntEvent;
	}

	private void _onWireStateChanged(int port) {
		if (_getIsPortActive (port)) {

		} else {

		}
		if (_activePort.port1 == -1) {
			_activePort.port1 = port;
		} else {
			if (_getIsValidPortCombination (port)) {
				_activePort.port2 = port;

			}
		}
	}

	private bool _getIsPortActive(int port) {
		if (_activePort.port1 == port || _activePort.port2 == port) {
			return true;
		}
		return false;
	}

	private bool _getIsValidPortCombination(int port) {
		bool isAdjacent = false;

		CellPosition pos1 = _setCellPosition (_activePort.port1);
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

	private void OnDestroy() {
		EventCenter ec = EventCenter.Instance; 
		if(ec != null) {
			ec.OnIntEvent -= OnIntEvent;
		}
	}

}

[Serializable]
public struct PortCombination {
	public int port1;
	public int port2;
	public bool isActivated;
}
