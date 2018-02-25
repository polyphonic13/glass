namespace Polyworks
{
	using System;
	using UnityEngine;

	public class AxisRotationAgent: MonoBehaviour
	{
		public Vector3 axisIncrements = new Vector3(0, 0, 0);

		public Vector3 initialRotation;

		public float speed;

		private Vector3 _nextRotations;

		private bool _isAnimating = false; 

		public void SetRotation(float value)
		{
			float x = -(axisIncrements.x * value);
			float y = -(axisIncrements.y * value);
			float z = -(axisIncrements.z * value);

			_nextRotations = new Vector3(x, y, z);
			Debug.Log ("AxisRotationAgent/Rotate, value = " + value + ", _nextRotations = " + _nextRotations);
//			transform.Rotate (_nextRotations);
//			transform.Rotate(_nextRotations.x, _nextRotations.y, _nextRotations.z);
			this.transform.eulerAngles = _nextRotations;
//			_isAnimating = true;
		}

//		private void FixedUpdate()
//		{
//			if (_isAnimating) 
//			{
//				if (Mathf.Abs(transform.eulerAngles.x) == Mathf.Abs(_nextRotations.x) && Mathf.Abs(transform.eulerAngles.y) == Mathf.Abs(_nextRotations.y) && Mathf.Abs(transform.eulerAngles.z) == Mathf.Abs(_nextRotations.z)) 
//				{
//					Debug.Log (" reached the desired rotation");
//					_isAnimating = false;
//					return;
//				}
//
//				transform.Rotate (_nextRotations * Time.deltaTime * speed);
//			}
//		}
	}
}