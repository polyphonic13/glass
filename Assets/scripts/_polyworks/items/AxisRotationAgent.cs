namespace Polyworks
{
	using System;
	using UnityEngine;

	public class AxisRotationAgent: MonoBehaviour
	{
		public Vector3 axisIncrements = new Vector3(0, 0, 0);

		public Vector3 initialRotation;

		public float speed;

		public bool isLogOn = false;

		private Vector3 _targetRotations;
		private float _currentValue = 0;

		private bool _isAnimating = false; 

		public void SetRotation(float value)
		{
			Log("AxisRotationAgent["+this.name+"]/SetRotation, value = " + value);
			// reset from current value
			float x = (_currentValue * axisIncrements.x);
			float y = (_currentValue * axisIncrements.y);
			float z = (_currentValue * axisIncrements.z);

			_targetRotations = new Vector3(x, y, z);
			Log(" reset = " + _targetRotations);

			transform.Rotate(_targetRotations, Space.Self);

			// adjust to new value
			x = -(value * axisIncrements.x);
			y = -(value * axisIncrements.y);
			z = -(value * axisIncrements.z);

			_targetRotations = new Vector3(x, y, z);
			Log(" new = " + _targetRotations);

			transform.Rotate (_targetRotations, Space.Self);
//			transform.Rotate(_targetRotations.x, _targetRotations.y, _targetRotations.z);
//			this.transform.eulerAngles = _targetRotations;
//			_isAnimating = true;
			_currentValue = value;
		}

		public void Log(string message)
		{
			if(isLogOn)
			{
				Debug.Log(message);
			}
		}
//		private void FixedUpdate()
//		{
//			if (_isAnimating) 
//			{
//				if (Mathf.Abs(transform.eulerAngles.x) == Mathf.Abs(_targetRotations.x) && Mathf.Abs(transform.eulerAngles.y) == Mathf.Abs(_targetRotations.y) && Mathf.Abs(transform.eulerAngles.z) == Mathf.Abs(_targetRotations.z)) 
//				{
//					Debug.Log (" reached the desired rotation");
//					_isAnimating = false;
//					return;
//				}
//
//				transform.Rotate (_targetRotations * Time.deltaTime * speed);
//			}
//		}
	}
}