using UnityEngine;
using System.Collections;

public class StaminaManager : MonoBehaviour {

	public static bool isBoosted = false;

	private float _nextActionTime = 0.0f;
	private float _rechargeDelay = 5f;

	private float _maxStamina;
	private float _remainingStamina; 

	void Awake () {
		_remainingStamina = _maxStamina = GameControl.Instance.stamina;
		GameControl.Instance.updateStamina(_remainingStamina);
	}

	void Update () {
		if(Input.GetKey(KeyCode.LeftShift)) {
			if(_remainingStamina > 0) {
				isBoosted = true;
				_remainingStamina -= Time.deltaTime;
				GameControl.Instance.updateStamina(_remainingStamina);
			} else {
//				Debug.Log("out of stamina");
				isBoosted = false;
				_remainingStamina = 0;
			}
			_nextActionTime = Time.time + _rechargeDelay;
		} else {
			isBoosted = false;
			if(_remainingStamina < _maxStamina) {

				if(Time.time > _nextActionTime) {
//					Debug.Log("incrementing _remainingStamina: " + _remainingStamina + ", max = " + _maxStamina);
					_nextActionTime = Time.time + _rechargeDelay;

					Mathf.Floor(_remainingStamina++);
					if(_remainingStamina > _maxStamina) {
						_remainingStamina = _maxStamina;
					}
					GameControl.Instance.updateStamina(_remainingStamina);
				}
			}
		}
	}
	
}
