using UnityEngine;
using System.Collections;

public class BreathingManager : MonoBehaviour {

	public float breathHoldTime = 200.0f;
	public float damageForBreathlessSeconds = 5.0f;

	private bool _isUnderWater = false;
	private bool _isOutOfBreath = false;

	private float _timeUnderWater = 0;
	// Use this for initialization
	void Awake () {
		EventCenter.Instance.onUnderWater += this.onUnderWater;
	}
	
	// Update is called once per frame
	void Update () {
		if(_isUnderWater) {
			_timeUnderWater += Time.deltaTime;

			if(_timeUnderWater > breathHoldTime) {
				// dispatch damage taken
			}
		}
	}

	void onUnderWater(bool under) {
		_isUnderWater = under;
		if(!under) {
			_timeUnderWater = 0;
		}
	}
}
