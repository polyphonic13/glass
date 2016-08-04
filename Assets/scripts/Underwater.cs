using UnityEngine;
// using System.Collections;

public class Underwater : RoomElement {
	public Transform _player;
	public float _heightOffset = 1f;

	private bool _isUnderWater;
	private bool _previousState;
	private float _waterLevel;

	void Start() {
		_waterLevel = transform.position.y;
		_isUnderWater = false;
		_previousState = false;
		setNormal();
	}
	
	void Update() {
		if(IsRoomActive) {
			_isUnderWater =((_player.position.y + _heightOffset) < _waterLevel) || false;

			if(_isUnderWater != _previousState) {
                if(_isUnderWater) {
                    setUnderwater();
                } else {
                    setNormal();
                }
            }
            _previousState = _isUnderWater;
		}
	}
	
	void setNormal() {
//		EventCenter.Instance.ChangeUnderWater(false);
	}
	
	void setUnderwater() {
//		EventCenter.Instance.ChangeUnderWater(true);
	}
}
