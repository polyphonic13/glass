using UnityEngine;

public class FirstPersonMovement : MonoBehaviour {

	enum _movementStates { Normal, Climb, Swim, Crawl };
	static _movementStates _currentMovementType;

	void Awake() {
		_currentMovementType = _movementStates.Normal;
	}

	void Update() {

		switch(_currentMovementType) {
		case _movementStates.Normal:
			NormalMove();
			break;
		case _movementStates.Climb:
			ClimbMove();
			break;
		case _movementStates.Swim:
			SwimMove();
			break;
		}
	}

	void NormalMove() {
		// Debug.Log("normal move");
	}

	void ClimbMove() {
		// Debug.Log("climb move");

	}

	void SwimMove() {
		// Debug.Log("swim move");

	}
	
	void Crawl() {
		// Debug.Log("crawl move");

	}
}
