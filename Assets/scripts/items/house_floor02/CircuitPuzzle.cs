using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polyworks; 

public class CircuitPuzzle : Puzzle 
{
	public int[] solution;

	public string[] removeOnDeactivateItemPaths;

	public List<PuzzleWire> wireChildren { get; set; }

	public virtual void OnIntEvent(string type, int value) 
	{
		// Log ("CircuitPuzzle/OnIntEvent, type = " + type + ", value = " + value);

		switch (type) 
		{
			case "insert_wire":
				ToggleWireInserted (value, true, true);
				break;

			case "remove_wire":
				ToggleWireInserted (value, false, true);
				break;
		}
	}

	public override void Init() 
	{
		base.Init();
	}

	public override void Activate() 
	{
		base.Activate ();

		if (!isCompleted) 
		{
			EventCenter.Instance.OnIntEvent += OnIntEvent;
		}
	}

	public override void Deactivate () 
	{
		base.Deactivate ();

		if (!isCompleted) 
		{
			RemoveAllWires ();

			if (removeOnDeactivateItemPaths.Length > 0) 
			{
				foreach(string path in removeOnDeactivateItemPaths) 
				{
					Inventory inventory = Game.Instance.GetPlayerInventory ();
					inventory.AddFromPrefabPath (path);
				}
			}
		}
		RemoveListeners();
	}

	public override void Solve() 
	{
		base.Solve();
		RemoveListeners();
	}

	public virtual void RemoveAllWires() 
	{
		for (int i = 0; i < wireChildren.Count; i++) 
		{
			ToggleWireInserted (i, false);
		}
	}

	public virtual void ToggleWireInserted(int index, bool isInserted, bool isCheckIsSolved = false) 
	{
		Log("CircuitPuzzle["+this.name+"]/ToggleWireInserted, index = " + index + ", isInserted = " + isInserted + ", count = " + wireChildren.Count);
		if (index >= wireChildren.Count) 
		{
			return;
		}
		int childIndex = GetWireByIndex (index, wireChildren);

		if (childIndex == -1) 
		{
			return;
		}
		PuzzleWire wire = wireChildren [childIndex];

		if (isInserted) 
		{
			foreach (int s in wire.siblings) 
			{
				PuzzleWire sibling = wireChildren[s];
				sibling.isActivated = false;
				sibling.gameObject.SetActive (false);
				wireChildren [s] = sibling;
			}
		}

		wire.gameObject.SetActive (isInserted);
		wire.isActivated = isInserted;
		wireChildren [childIndex] = wire;

		if(isCheckIsSolved) 
		{
			CheckIsSolved ();
		}
	}

	public virtual void CheckIsSolved() 
	{
		isSolved = true;
		for (int i = 0; i < solution.Length; i++) 
		{

			Log ("solution[" + i + "] = " + solution [i] + " is activated = " + wireChildren [solution [i]].isActivated);

			if(!wireChildren[solution[i]].isActivated) 
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

	public virtual void RemoveListeners() 
	{
		EventCenter ec = EventCenter.Instance; 
		if(ec != null) 
		{
			ec.OnIntEvent -= OnIntEvent;
		}
	}

	public int GetWireByIndex(int index, List<PuzzleWire> list) 
	{
		for (int i = 0; i < list.Count; i++) 
		{
			if (list [i].index == index) 
			{
				return i;
			}
		}
		return -1;
	}

	private void OnDestroy() 
	{
		RemoveListeners();
	}
}

[Serializable]
public struct PuzzleWire 
{
	public int index;
	public GameObject gameObject;
	public bool isActivated;
	public List<int> siblings;
}
