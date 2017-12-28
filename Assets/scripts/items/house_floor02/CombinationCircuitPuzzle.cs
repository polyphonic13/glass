using UnityEngine;
using System.Collections;

public class CombinationCircuitPuzzle : Puzzle
{
	public int[] pegsTopRow;
	public int[] pegsBottomRow; 

	private Light _redLight;
	private Light _greenLight;


	public override void Init() {
//		Debug.Log ("CombinationCircuitPuzzle["+this.name+"]/Init");
		_redLight = GameObject.Find ("red_light").GetComponent<Light>();
		_greenLight = GameObject.Find ("green_light").GetComponent<Light>();

		_redLight.enabled = false; 
		_greenLight.enabled = false; 

		base.Init ();
	}
}

