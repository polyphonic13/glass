using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class MovingPlatform : AnimationAgent {
		public Transform[] destinations;
		public Transform startingPosition;
		public Transform platform;

		public float speed = 2.0f;
		public bool isAuto = true; 
		public bool isLooper = false;

		private int _currentDestination = 0; 

		private Transform _destination;
		private Vector3 _direction; 

		private bool _isMoving = false;

		public override void Play(string clip = "") {
			if (!_isMoving) {
				base.Play ();
				StartMovement ();
			}
		}

		public override void Pause() {
			base.Pause();
			_isMoving = false;
		}

		public override void Resume() {
			base.Resume();
			_isMoving = true;
		}

		public void SetDestination(bool isActive = false) {
			_destination = destinations [_currentDestination];
			_direction = (_destination.position - platform.transform.position).normalized;
			this.isActive = _isMoving = isActive;
		}

		public void StartMovement() {
			if (_destination != null) {
				isActive = true;
				_isMoving = true;
			}
		}

		public void Actuate() {
			if (!_isMoving) {
				StartMovement ();
			}
		}
		
		private void Awake () {
			SetDestination (isAuto);
			if (isAuto) {
				StartMovement ();
			}
		}

		private void FixedUpdate () {
			if (isActive && _isMoving) {
				platform.transform.Translate(_direction * speed * Time.fixedDeltaTime);

				if (Vector3.Distance (platform.transform.position, _destination.position) < speed * Time.fixedDeltaTime) {
					platform.transform.position = _destination.position;
					isActive = false;
					_destination = null;

					if (_currentDestination < (destinations.Length - 1)) {
						_currentDestination++;
					} else {
						_currentDestination = 0;
					}

					if (isAuto || isLooper) {
						SetDestination (true);
					} else {
						SetDestination (false);
					}
				}
			}
		}

		private void OnDrawGizmos() {
			for (int i = 0; i < destinations.Length; i++) {
				if(destinations[i] != null && platform != null) {
					Gizmos.DrawWireCube (destinations [i].position, platform.localScale);
				}
			}
		}
	}
}