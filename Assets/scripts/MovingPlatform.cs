using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {
	[SerializeField] Transform startingPosition;

	[SerializeField] Transform[] destinations;
	[SerializeField] Transform platform;

	[SerializeField] float speed = 2.0f;
	[SerializeField] bool isAuto = true; 
	[SerializeField] bool isLooping = false;

	private int _currentDestination = 0; 

	Transform _destination;
	Vector3 _direction; 

	bool _isActive = false; 
	bool _isMoving = false;

	public bool GetIsActive() {
		return _isActive;
	}

	public void SetDestination(bool setActive = false) {
		_destination = destinations [_currentDestination];
//		Debug.Log ("MovingPlatform[" + this.name + "]/SetDestination, pos = " + platform.transform.position + ", dest = " + _destination.position);
		_direction = (_destination.position - platform.transform.position).normalized;
		_isActive = setActive;
	}

	public void StartMovement() {
//		Debug.Log ("MovingPlatform/StartMovement, _destination = " + _destination.position);
		if (_destination != null) {
			_isActive = true;
			_isMoving = true;
		}
	}

	public void Pause() {
		_isActive = false;
	}

	public void Resume() {
		_isActive = true;
	}

	public void Actuate() {
		if (!_isMoving) {
			StartMovement ();
		}
	}
		
	void Awake () {
		SetDestination (isAuto);
		if (isAuto) {
			StartMovement ();
		}
	}

	void FixedUpdate () {
		if (_isActive) {
			platform.transform.Translate(_direction * speed * Time.fixedDeltaTime);

			if (Vector3.Distance (platform.transform.position, _destination.position) < speed * Time.fixedDeltaTime) {
				platform.transform.position = _destination.position;
				_isActive = false;
				_isMoving = false;	
				_destination = null;
//				Debug.Log ("reached destination");
				if (_currentDestination < (destinations.Length - 1)) {
					_currentDestination++;
				} else {
					_currentDestination = 0;
				}

				if (isAuto || isLooping) {
					SetDestination (true);
				} else {
					SetDestination (false);
				}
			}
		}
	}

	void OnDrawGizmos() {
		for (int i = 0; i < destinations.Length; i++) {
			if(destinations[i] != null && platform != null) {
				Gizmos.DrawWireCube (destinations [i].position, platform.localScale);
			}
		}
	}

}
