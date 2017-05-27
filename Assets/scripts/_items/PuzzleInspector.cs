using UnityEngine;
using System.Collections;
using System;
using UnitySampleAssets.Characters.FirstPerson;
using Polyworks; 

public class PuzzleInspector : ItemDetectionRaycastAgent, IInputControllable {

	#region members
	public PuzzleLocation[] locations;

	[SerializeField] private Light _light;
	[SerializeField] private Camera _camera;
	[SerializeField] private float _minX; 
	[SerializeField] private float _maxX;
	[SerializeField] private float _minY;
	[SerializeField] private float _maxY;
	[SerializeField] private float _xSpeed = 120.0f;
	[SerializeField] private float _ySpeed = 120.0f;

	private float _rotationYAxis = 0.0f;
	private float _rotationXAxis = 0.0f;

	private float _velocityX = 0.0f;
	private float _velocityY = 0.0f;

	private float _activeRotationY;
	private int _activeLocation = -1;

	private InputObject _input;

	#endregion

	#region event handlers
	public void OnContextChange(InputContext context, string param) {
		if (context == InputContext.PUZZLE) {
			string target = param.Substring (0, 8);
			Debug.Log ("PuzzleInspector/OnContextChange, context = " + context);
			int index = _getLocationIndex (target);
			if (index > -1) {
				Debug.Log (" has location, going to activate with index: " + index);
				Activate (index);
			}
		} else if (this.isActive) {
			this.isActive = false; 
		}
	}
	#endregion

	#region static methods
	public static float ClampAngle(float angle, float min, float max) {
		if (angle < -360F) {
			angle += 360F;
		}
		if (angle > 360F) {
			angle -= 360F;
		}
		return Mathf.Clamp(angle, min, max);
	}
	#endregion

	#region public methods
	public void Init() {
		EventCenter ec = EventCenter.Instance;
		ec.OnContextChange += this.OnContextChange;
	}

	public void Activate(int index) {
		if (index != _activeLocation) {
			// move the inspector to new location
			_setLocation (index);
		}
		_toggleActivated (true);
	}

	public void Deactivate() {
		_toggleActivated (false);
	}

	public void SetInput(InputObject input) {
		if (input.buttons ["cancel"]) {
			_toggleActivated (false);
			EventCenter.Instance.ChangeContext(InputContext.PLAYER, "");
		} else {
			if (input.horizontal != 0 || input.vertical != 0) {
				_rotate (input.horizontal, input.vertical);
			}
//			Debug.Log ("PuzzleInspector/SetInput, _horizonal = " + _horizontal + ", _vertical = " + _vertical);
		}
	}
	#endregion

	#region private methods
	private void Awake () {
		_toggleActivated (false);
		Init ();
	}

	private void Update() {
		if (this.isActive) {
//			_rotateView ();
			CheckRayCast ();
		}
	}

	private int _getLocationIndex(string name) {
		for (int i = 0; i < locations.Length; i++) {
			if (locations [i].name == name) {
				return i;
			}
		}
		return -1;
	}

	private void _setLocation(int index) {
		PuzzleLocation location = locations [index];
		this.transform.position = new Vector3 (location.position.x, location.position.y, location.position.z);
		this.transform.Rotate(location.rotation);
		_activeRotationY = location.rotation.y;
		_activeLocation = index;
	}

	private void _toggleActivated(bool isActivated) {
		Debug.Log ("PuzzleInspector/_toggleActivated, isActivated = " + isActivated);
		this._camera.enabled = isActivated; 
		this._light.enabled = isActivated;
		this.isActive = isActivated;
	}

	private void _rotate(float horizontal, float vertical) {
		Debug.Log ("PuzzleInspector/_rotate, x/y = " + horizontal + "/" + vertical + ", rot = " + this.transform.rotation.x + "/" + this.transform.rotation.y);

		_velocityX = _xSpeed * horizontal * 0.01f;
		_velocityY = _ySpeed * vertical * 0.01f;

		_rotationYAxis += _velocityX;
		_rotationXAxis -= _velocityY;
		_rotationXAxis = ClampAngle (_rotationXAxis, _minX, _maxX);
		_rotationYAxis = ClampAngle(_rotationYAxis, _minY, _maxY);
		Quaternion rotation = Quaternion.Euler(_rotationXAxis, _rotationYAxis + _activeRotationY, 0);

		transform.rotation = rotation;
	}

	private void _rotateView() {
	}

	private void OnDestroy() {
		EventCenter ec = EventCenter.Instance;
		if (ec != null) {
			ec.OnContextChange -= this.OnContextChange;
		}
	}
	#endregion
}

[Serializable]
public struct PuzzleLocation {
	public string name;
	public Vector3 position;
	public Vector3 rotation;
}