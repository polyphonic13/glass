using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polyworks; 

public class FuseCircuitPuzzle : CircuitPuzzle
{
	public int numColumns; 
	public int verticalPositions = 3;

	public RotatingDialGroup dials; 

	private List<List<int>> _wires;

	public override void Init() 
	{
		base.Init();
		_initPuzzleWires();
	}

	private void _initPuzzleWires() 
	{
		Log("FuseCircuitPuzzle["+this.name+"]/_initPuzzleWires, numColumns = " + numColumns + ", verticalPositions = " + verticalPositions); 
		int i;
		List<int> siblings;

		_wires = new List<List<int>>();

		for(i = 0; i < numColumns; i++)
		{
			siblings = new List<int>();

			int pos = i % verticalPositions	;

			if(pos == 0) 
			{
				if(i > 1) 
				{
					siblings.Add(i - 1);
					siblings.Add(i - 2);
				}
				if(i < numColumns - 2)
				{
					siblings.Add(i + 1);
					siblings.Add(i + 2);
				}
			}
			else if(pos == 1)
			{
				siblings.Add(i - 1);
				siblings.Add(i + 2);
			}
			else if(pos == 2)
			{
				siblings.Add(i + 1);
				siblings.Add(i - 2);
			}
			Log("sibling["+i+"] = " + siblings);
			_wires.Add(siblings);
		}
	}
}
