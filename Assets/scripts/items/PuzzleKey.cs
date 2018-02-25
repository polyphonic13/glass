using UnityEngine;
using System.Collections;
using Polyworks;
using System.Collections.Generic;

public class PuzzleKey : CollectableItem
{
	public string collectedFlag; 

	public override void Actuate ()
	{
		EventCenter.Instance.InvokeStringEvent(Puzzle.UNLOCK_EVENT, this.name);
		base.Actuate ();
	}

	public override void Use() {
		Debug.Log ("PuzzleKey[" + this.name + "]/Use");
	}
}

