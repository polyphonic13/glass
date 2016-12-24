using UnityEngine;
using System.Collections;
using System;

using Polyworks; 

public class DirectionalCircuitPuzzlePort : Item
{
	public enum PortState { Idle, Hover, Selecting, Connected } 

	public PortState State;
	public PortNeighbors Neighbors;

	public override void Actuate ()
	{
		base.Actuate ();
	}

	public override void SetEnabled (bool isEnabled)
	{
		base.SetEnabled (isEnabled);
	}

	private DirectionalCircuitPuzzle _puzzle; 

	private void Awake() {
		GameObject _puzzleParent = this.transform.parent.transform.parent.transform.parent.gameObject;
		_puzzle = _puzzleParent.GetComponent<DirectionalCircuitPuzzle> ();
		Debug.Log ("DirectionCircuitPuzzlePort[" + this.name + "]/Awake, _puzzle = " + _puzzle);
	}

}

[Serializable]
public struct PortNeighbors {
	public DirectionalCircuitPuzzlePort North;
	public DirectionalCircuitPuzzlePort South;
	public DirectionalCircuitPuzzlePort East;
	public DirectionalCircuitPuzzlePort West;

}
