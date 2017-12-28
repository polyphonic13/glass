using UnityEngine;
using Polyworks;

public class CameraZoom : MonoBehaviour {

	public float _zoom = 10;

	private Camera _camera;
	private float _normal;
	private bool _isZoomed;

	public void Execute() {
		_zoomCamera ();
	}

	public void OnMainCameraEnabled() {
		_camera = Camera.main;
		_normal = _camera.fieldOfView;
	}

	private void Awake() {
		EventCenter.Instance.OnMainCameraEnabled += OnMainCameraEnabled;
	}
		
	private void _zoomCamera() {
		_camera.fieldOfView =(_isZoomed) ? _normal : _zoom;
		_isZoomed = !_isZoomed;
	}

	private void OnDestroy() {
		EventCenter ec = EventCenter.Instance;
		if (ec != null) {
			ec.OnMainCameraEnabled -= OnMainCameraEnabled;
		}
	}
}
