using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polyworks; 

public class FuseCircuitPuzzle : CircuitPuzzle
{
	public int numColumns; 
	public int verticalPositions = 3;

	public float[] wireValues;
	public float initialDialValue;
	public RotatingDialGroup dials; 

	private int currentValue;
	public override void Init() 
	{
		base.Init();
		base.InitWires();
		dials.SetValues(initialDialValue);
	}

	public override List<int> GetWireSiblings(int index)
	{
		Log("FuseCircuitPuzzle["+this.name+"]/GetWireSiblings, index = " + index);
		List<int> siblings = new List<int>(); 

		int pos = index % verticalPositions;

		if(pos == 0) 
		{
			if(index > 1) 
			{
				Log(" pos 0, adding " + (index - 1) + ", " + (index - 2));
				siblings.Add(index - 1);
				siblings.Add(index - 2);
			}
			if(index < numColumns - 2)
			{
				Log(" pos 0, adding " + (index + 1) + ", " + (index + 2));
				siblings.Add(index + 1);
				siblings.Add(index + 2);
			}
		}
		else if(pos == 1)
		{
			Log(" pos 1, adding " + (index - 1) + ", " + (index + 2));
			siblings.Add(index - 1);
			siblings.Add(index + 2);
		}
		else if(pos == 2)
		{
			Log(" pos 1, adding " + (index + 1) + ", " + (index - 2));
			siblings.Add(index + 1);
			siblings.Add(index - 2);
		}

		return siblings;
	}

	// public override void ToggleWireInserted(int index, bool isInserted)
	// {
	// 	base.ToggleWireInserted(index, isInserted);
	// 	// UpdateCurrentValue();
	// }

	public void UpdateCurrentValue()
	{
		float currentValue = 0;

		for(int i = 0; i < wireChildren.Count; i++)
		{
			if(wireChildren[i].isActivated)
			{
				currentValue += wireValues[i];
			}
		}

		dials.SetValues(currentValue);
	}
}
