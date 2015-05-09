using UnityEngine;
using System.Collections;

public class BreathingManager : MonoBehaviour {

	private float _breathHoldTime;
	public float damageForBreathlessSeconds = 0.001f;

	private bool _isUnderWater = false;

	private float _timeUnderWater = 0;
	// Use this for initialization
	void Awake () {
		EventCenter.instance.onUnderWater += this.onUnderWater;
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
				GameControl.instance.damagePlayer(damage/100);
				// dispatch damage taken
			}
		}
	}

	void onUnderWater(bool under) {
//		Debug.Log("BreathingManager/onUnderWater, under = " + under);
		_isUnderWater = under;
		if(!under) {
			_timeUnderWater = 0;
			GameControl.instance.resetBreath();
		}
	}
}
