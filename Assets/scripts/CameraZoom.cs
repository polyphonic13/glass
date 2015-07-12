using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {

	public float zoom = 10;

	Camera _camera;
	float _normal;
//	float _smooth = 0.5f;
	bool _isZoomed = false;

	// Use this for initialization
	void Start () {
		_camera = GameObject.Find("FirstPersonCharacter").GetComponent<Camera>();
		_normal = _camera.fieldOfView;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Z)) {
			ZoomCamera();
		}
	}

	void ZoomCamera() {
		if(_isZoomed) {
//			_camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _normal, Time.deltaTime / _smooth);
			_camera.fieldOfView = _normal;
		} else {
//			_camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, zoom, Time.deltaTime / _smooth);
			_camera.fieldOfView = zoom;
		}
		_isZoomed = !_isZoomed;
	}
}
