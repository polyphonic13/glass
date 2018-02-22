using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polyworks; 

public class CircuitPuzzle : Puzzle 
{
	public GameObject[] objectsToRemoveOnDeactive;
	
	protected List<PuzzleWire> _wireChildren;

	public virtual void OnIntEvent(string type, int value) {
		Log ("DirectionalPuzzle/OnIntEvent, type = " + type + ", value = " + value);

		switch (type) {
		case "insert_wire":
			ToggleWireInserted (value, true);
			break;

		case "remove_wire":
			ToggleWireInserted (value, false);
			break;
		}
	}

	public override Init() 
	{
		base.Init();
	}

	protected override void Activate() {
		base.Activate ();

		if (!isCompleted) {
			EventCenter.Instance.OnIntEvent += OnIntEvent;
		}
	}

	protected override void Deactivate () {
		base.Deactivate ();

		if (!isCompleted) {
			_removeAllWires ();

			if (objectsToRemoveOnDeactive != "") {
				Inventory inventory = Game.Instance.GetPlayerInventory ();
				inventory.AddFromPrefabPath (objectsToRemoveOnDeactive);
			}
		}
		_removeListeners();
	}

	protected virtual void RemoveAllWires() 
	{
		for (int i = 0; i < _wireChildren.Count; i++) 
		{
			ToggleWireInserted (i, false);
		}
	}

	protected virtual void ToggleWireInserted(int index, bool isInserted) 
	{
		if (index >= _wireChildren.Count) 
		{
			return;
		}
		int childIndex = _getWireByIndex (index, _wireChildren);

		if (childIndex == -1) 
		{
			return;
		}
		PuzzleWire wire = _wireChildren [childIndex];

		if (isInserted) 
		{
			foreach (int s in wire.siblings) 
			{
				PuzzleWire sibling = _wireChildren[s];
				sibling.isActivated = false;
				sibling.gameObject.SetActive (false);
				_wireChildren [s] = sibling;
			}
		}

		wire.gameObject.SetActive (isInserted);
		wire.isActivated = isInserted;
		_wireChildren [childIndex] = wire;

		CheckIsSolved ();
	}

	protected virtual void CheckIsSolved() 
	{
		isSolved = true;
		for (int i = 0; i < solution.Length; i++) 
		{

			Log ("solution[" + i + "] = " + solution [i] + " is activated = " + _wireChildren [solution [i]].isActivated);

			if(!_wireChildren[solution[i]].isActivated) 
			{
				isSolved = false;
			}
		}
		if (isSolved) 
		{
			Solve ();
		}
		Log ("CircuitPuzzle[" + this.name + "]/CheckIsSolved, isSolved = " + isSolved);
	}

	protected virtual void RemoveListeners() {
		EventCenter ec = EventCenter.Instance; 
		if(ec != null) {
			ec.OnIntEvent -= OnIntEvent;
		}
	}

	private void OnDestroy() {
		RemoveListeners();
	}

}

[Serializable]
public struct PuzzleWire {
	public int index;
	public GameObject gameObject;
	public bool isActivated;
	public List<int> siblings;
}
