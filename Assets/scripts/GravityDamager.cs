using UnityEngine;
using System.Collections;

public class GravityDamager : MonoBehaviour {

	public float damageForFallSeconds = 5f;
	public float _minDamagingFall = 1.5f;

	private float _timeInAir = 0f;
	private bool _isFalling = false; 

	void Update () {
		if(_isFalling) {
			_timeInAir += Time.deltaTime;
		}
	}

	public void beginFall() {
//		Debug.Log("GravitytDamager/beginFall");
		_isFalling = true;
	}

	public float endFall() {
//		Debug.Log("GravityDamager/endFall, damageMultiplier = " + damageForFallSeconds + ", _timeInAir = " + _timeInAir);
		float damage = 0;
		if(_isFalling) {
			if(_timeInAir > _minDamagingFall) {
				Debug.Log("damage = " + ((_timeInAir * 10) * damageForFallSeconds));

				damage = (_timeInAir * 10) * damageForFallSeconds;
			}

			_isFalling = false;
			_timeInAir = 0;
		}
		return damage;
	}

	public void cancelFall() {
		_isFalling = false;
		_timeInAir = 0;
	}
}