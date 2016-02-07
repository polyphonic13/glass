using UnityEngine;
using Rewired;

public class CameraZoom : MonoBehaviour {

	public float _zoom = 10;

	public Camera _camera;
	private float _normal;
	private bool _isZoomed;

	private Rewired.Player _controls; 

	void Awake() {
		_controls = ReInput.players.GetPlayer(0);
		_normal = _camera.fieldOfView;
	}
	
	void Update() {
		if(_controls.GetButtonDown("zoom")) {
			_zoomCamera();
		}
	}

	void _zoomCamera() {
		_camera.fieldOfView =(_isZoomed) ? _normal : _zoom;
		_isZoomed = !_isZoomed;
	}
}
