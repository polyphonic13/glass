using UnityEngine;

public class StaminaManager : MonoBehaviour {

	private const float RECHARGE_DELAY = 5f;

	private float _nextActionTime;

	private float _maxStamina;
	private float _remainingStamina; 

	public static bool IsBoosted { get; set; }

	void Awake () {
		_remainingStamina = _maxStamina = GameControl.Instance.RemainingStamina;
		GameControl.Instance.UpdateStamina(_remainingStamina);
	}

	void Update () {
		if(Input.GetKey(KeyCode.LeftShift)) {
			if(_remainingStamina > 0) {
				IsBoosted = true;
				_remainingStamina -= Time.deltaTime;
				GameControl.Instance.UpdateStamina(_remainingStamina);
			} else {
//				Debug.Log("out of stamina");
				IsBoosted = false;
				_remainingStamina = 0;
			}
			_nextActionTime = Time.time + RECHARGE_DELAY;
		} else {
			IsBoosted = false;
			if(_remainingStamina < _maxStamina) {

				if(Time.time > _nextActionTime) {
//					Debug.Log("incrementing _remainingStamina: " + _remainingStamina + ", max = " + _maxStamina);
					_nextActionTime = Time.time + RECHARGE_DELAY;

					Mathf.Floor(_remainingStamina++);
					if(_remainingStamina > _maxStamina) {
						_remainingStamina = _maxStamina;
					}
					GameControl.Instance.UpdateStamina(_remainingStamina);
				}
			}
		}
	}
	
}
