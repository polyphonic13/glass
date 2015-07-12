using UnityEngine;

public class BreathingManager : MonoBehaviour {

	private float _breathHoldTime;
	public float _damageForBreathlessSeconds = 0.001f;

	private bool _isUnderWater;

	private float _timeUnderWater;
	// Use this for initialization
	void Awake () {
		EventCenter.Instance.OnUnderWater += OnUnderWater;
		_breathHoldTime = GameControl.instance.breath;
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log("BreathingManager/update under water =  " + _isUnderWater + ", time = " + _timeUnderWater);
		if(_isUnderWater) {

			_timeUnderWater += Time.deltaTime;
			GameControl.instance.updateHeldBreathTime(_timeUnderWater);

			if(_timeUnderWater > _breathHoldTime) {
				var damage = _timeUnderWater - _breathHoldTime;
				GameControl.instance.DamagePlayer(damage/100);
				// dispatch damage taken
			}
		}
	}

	void OnUnderWater(bool under) {
//		Debug.Log("BreathingManager/OnUnderWater, under = " + under);
		_isUnderWater = under;
		if(!under) {
			_timeUnderWater = 0;
			GameControl.instance.resetBreath();
		}
	}
}
