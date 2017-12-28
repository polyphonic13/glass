using UnityEngine;
using System.Collections;
using Polyworks;

public class PuzzleKey : CollectableItem
{
	public string collectedFlag; 

	public override void Actuate ()
	{
		EventCenter.Instance.InvokeStringEvent(Puzzle.UNLOCK_EVENT, this.name);
		base.Actuate ();
	}
}

