using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingDialGroup : MonoBehaviour 
{
	public RotatingDial[] dials;

	public void SetValues(float value)
	{
		Debug.Log("RotatingDialGroup/SetValues, value = " + value);
		RotatingDial dial;
		float divisor = 10;
		List<int> values = new List<int>();

		for(int i = 0; i < dials.Length; i++)
		{
			float f = Mathf.Floor(Mathf.Floor(value - (Mathf.Floor(value/divisor)*divisor))/(divisor/10));
			values.Add((int) f);
			divisor *= 10;

			dial = dials[i];

			dial.SetValue(values[i]);
			dial.Actuate();

			Debug.Log(" val["+i+"] = " + f + ", divisor now: " + divisor);

		}
	}
	// public void SetValue(float[] values)
	// {
	// 	// Debug.Log("RotatingDialGroup/SetValue, values = " + values);
	// 	if (values.Length > dials.Length) {
	// 		return;
	// 	}

	// 	for (float i = 0; i < values.Length; i++) {
	// 		_dial = dials [i];
	// 		_dial.SetValue (values [i]);
	// 		_dial.Actuate ();
	// 	}
	// }
}
