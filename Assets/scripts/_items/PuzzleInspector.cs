using UnityEngine;
using System.Collections;
using System;
using UnitySampleAssets.Characters.FirstPerson;
using Polyworks; 

public class PuzzleInspector : MonoBehaviour, IInputControllable {

	#region members
	public PuzzleLocation[] locations;

	[SerializeField] private Light _light;
	[SerializeField] private Camera _camera;
	[SerializeField] private GameObject _raycastObject; 
	[SerializeField] private GameObject _icon;
	[SerializeField] private GameObject _objectIcon;

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

	private float _activeRotationY = 0;
	private int _activeLocation = -1;

	public bool isActive = false;

	private InputObject _input;
	private RaycastAgent _raycastAgent; 

	#endregion

	#region event handlers
	public void OnContextChange(InputContext context, string param) {
		Debug.Log ("PuzzleInspector/OnContextChange, context = " + context);
		if (context == InputContext.PUZZLE) {
			string target = param.Substring (0, 8);
//			Debug.Log ("PuzzleInspector/OnContextChange, context = " + context);
			int index = _getLocationIndex (target);
			if (index > -1) {
				Debug.Log (" has location, going to activate with index: " + index);
				Activate (index);
			}
		} else if (this.isActive) {
			this.isActive = false; 
		}
	}

	public void OnNearItem(Item item, bool isFocused) {
//		Debug.Log ("PuzzleInspector/OnNearItem, item = " + item.name + ", isFocused = " + isFocused);
	}
	#endregion

	#region public methods
	public void Init() {
		EventCenter ec = EventCenter.Instance;
		ec.OnContextChange += this.OnContextChange;
		_raycastAgent = _raycastObject.GetComponent<RaycastAgent> ();
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
//			Debug.Log ("PuzzleInspector/SetInput, horizontal = " + input.horizontal + ", vertical = " + input.vertical);
		}
	}
	#endregion

	#region private methods
	private void Awake () {
		Init ();
		_toggleActivated (false);
	}

	private void Update() {
//		Debug.Log ("PuzzleInspector/Update, isActive = " + this.isActive);
		if (this.isActive) {
			_raycastAgent.CheckRayCast ();
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
		if (_activeRotationY != 0) {
			Vector3 rotate = new Vector3 (0, -_activeRotationY, 0);
			transform.Rotate (rotate);
		}

		PuzzleLocation location = locations [index];
		transform.position = new Vector3 (location.position.x, location.position.y, location.position.z);
		transform.Rotate(location.rotation);
		_activeRotationY = location.rotation.y;
		_activeLocation = index;
	}

	private void _toggleActivated(bool isActivated) {
//		Debug.Log ("PuzzleInspector/_toggleActivated, isActivated = " + isActivated);
		EventCenter ec = EventCenter.Instance;

		if (isActivated) {
			ec.OnNearItem += OnNearItem;
		} else {
			ec.OnNearItem -= OnNearItem;
		}

		if (this.isActive && !isActivated) {
//			Debug.Log ("  was active, have to deactivate stuff");
			_raycastAgent.ClearFocus ();
			ec.InvokeStringEvent (Puzzle.ACTIVATE_EVENT);
		}
		_raycastObject.SetActive (isActivated);
		this.isActive = _camera.enabled = _light.enabled = isActivated;
	}

	private void _rotate(float horizontal, float vertical) {
		_velocityX = _xSpeed * horizontal * 0.01f;
		_velocityY = _ySpeed * vertical * 0.01f;

		_rotationYAxis += _velocityX;
		_rotationXAxis -= _velocityY;
		_rotationXAxis = Polyworks.Utils.ClampAngle(_rotationXAxis, _minX, _maxX);
		_rotationYAxis = Polyworks.Utils.ClampAngle(_rotationYAxis, _minY, _maxY);
		Quaternion rotation = Quaternion.Euler(_rotationXAxis, _rotationYAxis + _activeRotationY, 0);

		_raycastObject.transform.rotation = rotation;
	}

	private void _rotateView() {
	}

	private void OnDestroy() {
		EventCenter ec = EventCenter.Instance;
		if (ec != null) {
			ec.OnContextChange -= this.OnContextChange;
			ec.OnNearItem -= this.OnNearItem;
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