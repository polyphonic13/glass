using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingDialGroup : MonoBehaviour {

	public RotatingDial[] dials;

	private RotatingDial _dial;

	public void SetValue(int[] values)
	{
		// Debug.Log("RotatingDialGroup/SetValue, values = " + values);
		if (values.Length > dials.Length) {
			return;
		}

		for (int i = 0; i < values.Length; i++) {
			_dial = dials [i];
			_dial.SetValue (values [i]);
			_dial.Actuate ();
		}
	}
}
