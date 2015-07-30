using UnityEngine;

public class Camera_zoom : MonoBehaviour {

	public float _zoom = 10;

	Camera _camera;
	float _normal;
//	float _smooth = 0.5f;
	bool _isZoomed;

	// Use this for Initialization
	void Start() {
		_camera = GameObject.Find("FirstPersonCharacter").GetComponent<Camera>();
		_normal = _camera.fieldOfView;
	}
	
	// Update is called once per frame
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
