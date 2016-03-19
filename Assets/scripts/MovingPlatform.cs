using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {
	[SerializeField] Transform startingPosition;

	[SerializeField] Transform[] destinations;
	[SerializeField] Transform platform;

	[SerializeField] float speed = 2.0f;
	[SerializeField] bool loops = true; 

	private int _currentDestination = 0; 

	Rigidbody _rigidBody;
	Transform _destination;
	Vector3 _direction; 

	bool _isActive = false; 

	public bool GetIsActive() {
		return _isActive;
	}

	public void SetDestination() {
		_destination = destinations [_currentDestination];
		_direction = (_destination.position - _rigidBody.position).normalized;
	}

	public void StartMovement() {
		Debug.Log ("MovingPlatform/StartMovement, _destination = " + _destination.position);
		if (_destination != null) {
			_isActive = true;
		}
	}

	public void Actuate() {
		StartMovement ();
	}
		
	void Awake () {
		_rigidBody = platform.GetComponent<Rigidbody> ();
//		platform.position = new Vector3(0, 0, 0);
		SetDestination ();
//		Debug.Log ("startingPosition = " + startingPosition.position + ", platform = " + platform.position + ", _rigidbody = " + _rigidBody.position);
	}

//	void OnCollisionEnter(Collision col) {
//		Debug.Log ("MovingPlatform/OnCollisionEnter, col = " + col.gameObject.name);
//	}

	void OnDrawGizmos() {
		for (int i = 0; i < destinations.Length; i++) {
			if(destinations[i] != null && platform != null) {
				Gizmos.DrawWireCube (destinations [i].position, platform.localScale);
			}
		}
	}

	void FixedUpdate () {
		if (_isActive) {
			Debug.Log ("MovingPlatform/FixedUpdate, position = " + _rigidBody.position + ", dest = " + _destination.position);
			_rigidBody.MovePosition (_rigidBody.position + (_direction * speed * Time.fixedDeltaTime));
			if (Vector3.Distance (_rigidBody.position, _destination.position) < speed * Time.fixedDeltaTime) {
				_rigidBody.position = _destination.position;
				_isActive = false;
				_destination = null;
				if (_currentDestination < (destinations.Length - 1)) {
					_currentDestination++;
				} else if(loops) {
					_currentDestination = 0;
				}
				SetDestination ();
				// trigger something via event center? 
			}
		}
	}

}
