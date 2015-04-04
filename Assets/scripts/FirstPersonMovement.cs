using UnityEngine;
using System.Collections;

public class FirstPersonMovement : MonoBehaviour {

	enum _movementStates { normal, climb, swim };
	static _movementStates _currentMovementType;

	void Awake() {
		_currentMovementType = _movementStates.normal;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// determine move state first

		switch(_currentMovementType) {
		case _movementStates.normal:
			normalMove();
			break;
		case _movementStates.climb:
			climbMove();
			break;
		case _movementStates.swim:
			swimMove();
			break;
		}
	}

	void normalMove() {

	}

	void climbMove() {

	}

	void swimMove() {

	}
	
}
