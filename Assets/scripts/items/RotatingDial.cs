using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polyworks; 

public class RotatingDial : ActuateAgent 
{
	public int min;
	public int max;

	public float rotationDegree;

	private int _currentValue;
	private AxisRotationAgent _axisRotationAgent; 

	public void SetValue(int value)
	{
		if(value > max || value < min)
		{
			return;
		}
		_currentValue = value;
	}

	public override void Actuate()
	{
		Log ("RotatingDial[" + this.name + "]/Actuate, _currentValue = " + _currentValue);
		_axisRotationAgent.SetRotation(_currentValue);
	}

	private void Awake()
	{
		_axisRotationAgent = gameObject.GetComponent<AxisRotationAgent>();	
		_axisRotationAgent.axisIncrements = new Vector3(rotationDegree, 0, 0);
		Log ("RotatingDial[" + this.name + "]/Awake, rotationDegree = " + rotationDegree);

//		SetDialValue (7);
//		Actuate ();
	}

}
