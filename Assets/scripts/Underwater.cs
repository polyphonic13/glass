using UnityEngine;
using System.Collections;

public class Underwater : RoomElement {
	public Transform player;
	public float heightOffset = 1f;

	private bool _isUnderWater;
	private bool _previousState;
	private float _waterLevel;

	// Use this for initialization
	void Start () {
		_waterLevel = this.transform.position.y;
		_isUnderWater = false;
		_previousState = false;
		setNormal();
	}
	
	// Update is called once per frame
	void Update () {
		if(this.isRoomActive) {
//			Debug.Log("player y = " + player.position.y + ", water y = " + _waterLevel + ", under water = " + _isUnderWater);
			if((player.position.y + heightOffset) < _waterLevel) {
				_isUnderWater = true;
			} else {
				_isUnderWater = false;
			}
			
			//		if ((player.position.y < _waterLevel) != _isUnderWater) {
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
		Debug.Log("above water");
		EventCenter.Instance.changeUnderWater(false);
//		RenderSettings.fogDensity = 0.01f;
//		RenderSettings.fogDensity = 0;
		
		//  Testing
//		waterPlane.localScale = new Vector3 (waterPlane.localScale.x, 1.0f, waterPlane.localScale.z);
	}
	
	void setUnderwater () {
		Debug.Log("under water");
//		RenderSettings.fogDensity = 0.5f;
		EventCenter.Instance.changeUnderWater(true);
		
		//  Testing
//		waterPlane.localScale = new Vector3 (waterPlane.localScale.x, -1.0f, waterPlane.localScale.z);
	}
}
