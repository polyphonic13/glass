using UnityEngine;
using System.Collections;

public class CombinationCircuitPuzzle : Puzzle
{
	private Light _redLight;
	private Light _greenLight;

	public override void Init() {
		Debug.Log ("CombinationCircuitPuzzle/Init");
		_redLight = GameObject.Find ("red_light").GetComponent<Light>();
		_greenLight = GameObject.Find ("green_light").GetComponent<Light>();

		_redLight.enabled = false; 
		_greenLight.enabled = false; 

		base.Init ();
	}
}

