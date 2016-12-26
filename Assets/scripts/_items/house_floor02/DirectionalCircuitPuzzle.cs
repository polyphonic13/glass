using UnityEngine;
using System.Collections;
using System;

public class DirectionalCircuitPuzzle : Puzzle
{
	public PortCombination[] combinations; 

	public int columns; 
	public int rows;

	private int[] _activatedPorts; 

	public override void Init() {
		int total = columns * rows; 

		_activatedPorts = new int[total];
		for (int i = 0; i < total; i++) {
			_activatedPorts [i] = -1;
		}
		Debug.Log ("DirectionalCircuitPuzzle["+this.name+"]/Init, _activatedPorts = " + _activatedPorts.Length);
		base.Init ();
	}

	public void WireInserted(int port) {

	}

	public void WireRemoved(int port) {

	}
}

[Serializable]
public struct PortCombination {
	public int port1;
	public int port2;
}


