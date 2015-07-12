using UnityEngine;
using System.Collections;

public class StaminaManager : MonoBehaviour {

	public static bool isBoosted = false;

	private float _nextActionTime = 0.0f;
	private float _rechargeDelay = 5f;

	private float _maxStamina;
	private float RemainingStamina; 

	void Awake () {
		RemainingStamina = _maxStamina = GameControl.Instance.Rem;
		GameControl.Instance.UpdateStamina(RemainingStamina);
	}

	void Update () {
		if(Input.GetKey(KeyCode.LeftShift)) {
			if(RemainingStamina > 0) {
				isBoosted = true;
				RemainingStamina -= Time.deltaTime;
				GameControl.Instance.UpdateStamina(RemainingStamina);
			} else {
//				Debug.Log("out of stamina");
				isBoosted = false;
				RemainingStamina = 0;
			}
			_nextActionTime = Time.time + _rechargeDelay;
		} else {
			isBoosted = false;
			if(RemainingStamina < _maxStamina) {

				if(Time.time > _nextActionTime) {
//					Debug.Log("incrementing RemainingStamina: " + RemainingStamina + ", max = " + _maxStamina);
					_nextActionTime = Time.time + _rechargeDelay;

					Mathf.Floor(RemainingStamina++);
					if(RemainingStamina > _maxStamina) {
						RemainingStamina = _maxStamina;
					}
					GameControl.Instance.UpdateStamina(RemainingStamina);
				}
			}
		}
	}
	
}
