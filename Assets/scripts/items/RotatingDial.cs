using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Polyworks; 

public class RotatingDial : ActuateAgent 
{
	public int min;
	public int max;

	public float rotationDegree;

	private bool _isInitialized = false; 
	private int _currentValue;
	private AxisRotationAgent _axisRotationAgent; 

	public void SetValue(int value)
	{
		if(value > max || value < min)
		{
			return;
		}
		Log("RotationgDial["+this.name+"]/SetValue, value = " + value);
		_currentValue = value;
	}

	public override void Actuate()
	{
		Log ("RotatingDial[" + this.name + "]/Actuate, _currentValue = " + _currentValue);
		if (!_isInitialized) {
			Init ();
		}
		_axisRotationAgent.SetRotation(_currentValue);
	}

	public void Init()
	{
		if (_isInitialized) 
		{
			return;
		}
		_axisRotationAgent = gameObject.GetComponent<AxisRotationAgent>();	
		_axisRotationAgent.axisIncrements = new Vector3(rotationDegree, 0, 0);
		// Log ("RotatingDial[" + this.name + "]/Awake, rotationDegree = " + rotationDegree);
		_isInitialized = true;
	}
}
