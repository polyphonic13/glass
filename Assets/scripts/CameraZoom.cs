using UnityEngine;

public class CameraZoom : MonoBehaviour {

	public float _zoom = 10;

	public Camera _camera;
	float _normal;
	bool _isZoomed;

	void Start() {
		_normal = _camera.fieldOfView;
	}
	
	void Update() {
		if(Input.GetKeyDown(KeyCode.Z)) {
			_zoomCamera();
		}
	}

	void _zoomCamera() {
		_camera.fieldOfView = (_isZoomed) ? _normal : _zoom;
		_isZoomed = !_isZoomed;
	}
}
