using UnityEngine;
using Rewired;

public class StaminaManager : MonoBehaviour {

	private const float RECHARGE_DELAY = 5f;

	private float _nextActionTime;

	private float _maxStamina;
	private float _remainingStamina; 

	private Rewired.Player _controls;

	public static bool IsBoosted { get; set; }

	void Awake() {
		_controls = ReInput.players.GetPlayer(0);

//		_remainingStamina = _maxStamina = Game.Instance.RemainingStamina;
//		Game.Instance.UpdateStamina(_remainingStamina);
	}

	void Update() {
		if(_controls.GetButton("run")) {
			IsBoosted = true;
//			Debug.Log ("StaminaManager/Update, run control, _remaininStamina = " + _remainingStamina);
//			if(_remainingStamina > 0) {
//				IsBoosted = true;
//				_remainingStamina -= Time.deltaTime;
//				Game.Instance.UpdateStamina(_remainingStamina);
//			} else {
//				IsBoosted = false;
//				_remainingStamina = 0;
//			}
//			_nextActionTime = Time.time + RECHARGE_DELAY;
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
//					Game.Instance.UpdateStamina(_remainingStamina);
				}
			}
		}
	}
	
}
