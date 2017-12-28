using UnityEngine;
using System;
using System.Collections.Generic;
using Polyworks;

public class WirePuzzle : Puzzle
{
	public int[] solution; 

	public string wiresPath = "";

	public List<PuzzleWire> wireChildren;

	public void OnIntEvent(string type, int value) {
		//		Debug.Log ("DirectionalPuzzle/OnIntEvent, type = " + type + ", value = " + value);

		switch (type) {
		case "insert_wire":
			ToggleWireInserted (value, true);
			break;

		case "remove_wire":
			ToggleWireInserted (value, false);
			break;
		}
	}

	public override void Init() {
		base.Init ();
	}

	public virtual void InitPuzzleWires() {
		wireChildren = new List<PuzzleWire> ();

		if (wiresPath != "") {
			Transform wireHolder = transform.Find (wiresPath);
			int count = 0; 

			foreach(Transform t in wireHolder) {
				PuzzleWire puzzleWire = new PuzzleWire ();
				puzzleWire.gameObject = t.gameObject;
				puzzleWire.index = count;
				puzzleWire.isActivated = false;
				puzzleWire.gameObject.SetActive (false);
				wireChildren.Add (puzzleWire);

				count++;
			}
		}
	}

	public virtual void ToggleWireInserted(int value, bool isInserted) {}

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

	public int GetWireByIndex(int index, List<PuzzleWire> list) {
		for (int i = 0; i < list.Count; i++) {
			if (list [i].index == index) {
				return i;
			}
		}
		return -1;
	}


}

