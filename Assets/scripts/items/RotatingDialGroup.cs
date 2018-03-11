using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingDialGroup : MonoBehaviour 
{
	public RotatingDial[] dials;

	public void SetValues(float value)
	{
		// Debug.Log("RotatingDialGroup/SetValues, value = " + value);
		RotatingDial dial;
		float divisor = 10;

		for(int i = 0; i < dials.Length; i++)
		{
			float f = Mathf.Floor(Mathf.Floor(value - (Mathf.Floor(value/divisor)*divisor))/(divisor/10));
			divisor *= 10;

			dial = dials[i];

			dial.SetValue((int) f);
			dial.Actuate();

			// Debug.Log(" val["+i+"] = " + f);

		}
	}
}
