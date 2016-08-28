using UnityEngine;

public class GravityDamager : MonoBehaviour {

	public float _damageForFallSeconds = 5f;
	public float _minDamagingFall = 1.5f;

	private float _timeInAir;
	private bool _isFalling; 

	void Update() {
		if(_isFalling) {
			_timeInAir += Time.deltaTime;
		}
	}

	public void BeginFall() {
//		// Debug.Log("GravitytDamager/BeginFall");
		_isFalling = true;
	}

	public float EndFall() {
//		// Debug.Log("GravityDamager/EndFall, damageMultiplier = " + _damageForFallSeconds + ", _timeInAir = " + _timeInAir);
		float damage = 0f;
		if(_isFalling) {
			if(_timeInAir > _minDamagingFall) {
				// Debug.Log("damage = " +((_timeInAir * 10) * _damageForFallSeconds));

				damage =(_timeInAir * 10) * _damageForFallSeconds;
			}

			_isFalling = false;
			_timeInAir = 0;
		}
		return damage;
		// return 0;
	}

	public void CancelFall() {
		_isFalling = false;
		_timeInAir = 0;
	}
}