namespace Polyworks {
	using UnityEngine;
	using System.Collections;

	public class RotationFollowAgent : MonoBehaviour
	{
		public Transform target;
		public float speed = 0.5f;

		private Quaternion _targetRotation; 
		private float _strength; 

		private void LateUpdate() {
			_targetRotation = Quaternion.LookRotation (target.position - transform.position);
			_strength = Mathf.Min (speed * Time.deltaTime, 1);
			transform.rotation = Quaternion.Lerp (transform.rotation, _targetRotation, _strength);
		}
	}
}
