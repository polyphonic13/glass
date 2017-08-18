namespace Polyworks {
	using UnityEngine;
	using System.Collections;

	public class GameObjectRotator : ActuateAgent
	{
		public Transform[] targets;
		public Vector3[] rotations;

		public float turningRate = 0; 

		public bool isLooping = true; 
		public bool isLogOn;

		private int _rotationIndex = 0; 

		public override void Actuate() {
			_log ("GameObjectRotator[" + this.name + "]/Execute");
			if (targets.Length > 0) {
				_log ("  rotations[" + _rotationIndex + "] = " + rotations [_rotationIndex]);
				_rotateTargets (rotations [_rotationIndex]);
				if (_rotationIndex < rotations.Length - 1) {
					_rotationIndex++;
				} else if (isLooping) {
					_rotationIndex = 0;
				}

				EventCenter.Instance.InvokeIntEvent (this.name, _rotationIndex);
			}
		}

		public override void Use ()
		{
			Actuate ();
		}

		private void _rotateTargets(Vector3 rotation) {
			for (int i = 0; i < targets.Length; i++) {
				_log (" rotating targets[" + i + "]: " + targets [i]);
				if (turningRate == 0) {
					targets [i].Rotate (rotation);
				} else {
					_animateRotateTarget (rotation, targets [i]);
				}
			}
		}
		 
		private void _animateRotateTarget(Vector3 rotation, Transform target) {
			Quaternion targetRotation = Quaternion.Euler(rotation);
			_log ("GameObjectRotator/_animateRotateTarget, rotation = " + rotation + ", targetRotation = " + targetRotation + ", transform.rotation = " + target.rotation);
			target.rotation = Quaternion.RotateTowards (target.rotation, targetRotation, turningRate * Time.deltaTime);
		}

		private void _log(string message) {
			if (isLogOn) {
				Debug.Log (message);
			}
		}
	}
}