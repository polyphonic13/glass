using UnityEngine;
// using System.Collections;

public class Underwater : RoomElement {
	public Transform _player;
	public float _heightOffset = 1f;

	private bool _isUnderWater;
	private bool _previousState;
	private float _waterLevel;

	// Use this for initialization
	void Start () {
		_waterLevel = transform.position.y;
//		Debug.Log(this.name + ", _waterLevel = " + _waterLevel);
		_isUnderWater = false;
		_previousState = false;
		setNormal();
	}
	
	// Update is called once per frame
	void Update () {
		if(isRoomActive) {
//			Debug.Log(name + " _player y = " + _player.position.y + ", water y = " + _waterLevel + ", under water = " + _isUnderWater);
			_isUnderWater = ((_player.position.y + _heightOffset) < _waterLevel) || false;
			
			//		if ((_player.position.y < _waterLevel) != _isUnderWater) {
			//			_isUnderWater = transform.position.y < _waterLevel;
			//			if (_isUnderWater) setUnderwater ();
			//			if (!_isUnderWater) setNormal ();
			//		}
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
	
	void setNormal () {
//		Debug.Log("above water");
		EventCenter.Instance.ChangeUnderWater(false);
//		RenderSettings.fogDensity = 0.01f;
//		RenderSettings.fogDensity = 0;
		
		//  Testing
//		waterPlane.localScale = new Vector3 (waterPlane.localScale.x, 1.0f, waterPlane.localScale.z);
	}
	
	void setUnderwater () {
//		Debug.Log("under water");
//		RenderSettings.fogDensity = 0.5f;
		EventCenter.Instance.ChangeUnderWater(true);
		
		//  Testing
//		waterPlane.localScale = new Vector3 (waterPlane.localScale.x, -1.0f, waterPlane.localScale.z);
	}
}
