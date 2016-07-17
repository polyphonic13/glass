using UnityEngine;

public class BreathingManager : MonoBehaviour {

	private float _breathHoldTime;
	public float _damageForBreathlessSeconds = 0.001f;

	private bool _isUnderWater;

	private float _timeUnderWater;

	void Awake() {
//		EventCenter.Instance.OnUnderWater += OnUnderWater;
//		_breathHoldTime = Game.Instance.RemainingBreath;
	}
	
	void Update() {
		if(_isUnderWater) {

			_timeUnderWater += Time.deltaTime;
//			Game.Instance.UpdateHeldBreathTime(_timeUnderWater);

			if(_timeUnderWater > _breathHoldTime) {
				var damage = _timeUnderWater - _breathHoldTime;
//				Game.Instance.DamagePlayer(damage/100);
			}
		}
	}

	void OnUnderWater(bool under) {
		_isUnderWater = under;
		if(!under) {
			_timeUnderWater = 0;
//			Game.Instance.ResetBreath();
		}
	}
}
