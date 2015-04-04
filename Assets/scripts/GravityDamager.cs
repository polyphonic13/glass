using UnityEngine;
using System.Collections;

public class GravityDamager : MonoBehaviour {

	[SerializeField] private float _minDamagingFall = 1.5f;
	[SerializeField] private float _damageForFallSeconds = 5f;

	private float _timeInAir = 0f;
	private bool _isFalling = false; 

	void Update () {
		if(_isFalling) {
			_timeInAir += Time.deltaTime;
		}
	}

	public void beginFall() {
		Debug.Log("GravitytDamager/beginFall");
		_isFalling = true;
	}

	public float endFall() {
		Debug.Log("GravityDamager/endFall, _timeInAir = " + _timeInAir);
		float damage = 0;

		if(_timeInAir > _minDamagingFall) {
			Debug.Log("damage = " + (_timeInAir * _damageForFallSeconds));
//			damage = Mathf.Round(_timeInAir * _damageForFallSeconds);
			damage = _timeInAir * _damageForFallSeconds;
		}

		_isFalling = false;
		_timeInAir = 0;

		return damage;
	}
}