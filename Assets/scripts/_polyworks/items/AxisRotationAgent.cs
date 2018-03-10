namespace Polyworks
{
	using System;
	using UnityEngine;

	public class AxisRotationAgent: MonoBehaviour
	{
		public Vector3 axisIncrements = new Vector3(0, 0, 0);

		public Vector3 initialRotation;

		public float speed;

		private Vector3 _targetRotations;

		private bool _isAnimating = false; 

		public void SetRotation(float value)
		{
			float x = -(axisIncrements.x * value);
			float y = -(axisIncrements.y * value);
			float z = -(axisIncrements.z * value);

			_targetRotations = new Vector3(x, y, z);
			// Debug.Log ("AxisRotationAgent/Rotate, value = " + value + ", _targetRotations = " + _targetRotations);
			transform.Rotate (_targetRotations, Space.Self);
//			transform.Rotate(_targetRotations.x, _targetRotations.y, _targetRotations.z);
//			this.transform.eulerAngles = _targetRotations;
//			_isAnimating = true;
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