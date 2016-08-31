using UnityEngine;

public class CameraZoom : MonoBehaviour {

	public float _zoom = 10;

	private Camera _camera;
	private float _normal;
	private bool _isZoomed;

	public void Execute() {
		_zoomCamera ();
	}

	void Awake() {
		_camera = Camera.main;
		_normal = _camera.fieldOfView;
	}
		
	void _zoomCamera() {
		_camera.fieldOfView =(_isZoomed) ? _normal : _zoom;
		_isZoomed = !_isZoomed;
	}
}
