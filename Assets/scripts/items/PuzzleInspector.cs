using UnityEngine;
using System.Collections;
using System;
using UnitySampleAssets.Characters.FirstPerson;
using Polyworks; 

public class PuzzleInspector : MonoBehaviour, IInputControllable {

	#region members
	public Vector3 startingPosition = new Vector3(0, 0, 0);
	public Vector3 startingRotation = new Vector3(0, 0, 0);
	public PuzzleLocation[] locations;

	[SerializeField] private Light _light;
	[SerializeField] private Camera _camera;
	[SerializeField] private GameObject _raycastObject; 
	[SerializeField] private GameObject _icon;
	[SerializeField] private GameObject _objectIcon;

	[SerializeField] private float _xSpeed = 120.0f;
	[SerializeField] private float _ySpeed = 120.0f;

	private float _rotationYAxis = 0.0f;
	private float _rotationXAxis = 0.0f;

	private float _velocityX = 0.0f;
	private float _velocityY = 0.0f;

	private float _activeRotationY = 0;
	private int _activeLocation = -1;
	private Bounds _activeBounds; 
	private float _xDeviation;
	private float _yDeviation; 

	public bool isActive = false;
	public bool isLogOn = false;
	private InputObject _input;
	private RaycastAgent _raycastAgent; 

	#endregion

	#region event handlers
	public void OnContextChange(InputContext context, string param) 
	{
		Log ("PuzzleInspector/OnContextChange, context = " + context + ", param = " + param);
		if (context == InputContext.PUZZLE) {
//			Log ("  param = " + param);
			int index = _getLocationIndex (param);
			if (index > -1) {
				Log (" has location, going to activate with index: " + index);
				Activate (index);
			}
		} else if (this.isActive) {
			Deactivate();
		}
	}

	public void OnNearItem(Item item, bool isFocused) 
	{
//		Log ("PuzzleInspector/OnNearItem, item = " + item.name + ", isFocused = " + isFocused);
	}
	#endregion

	#region public methods
	public void Init() 
	{
		EventCenter ec = EventCenter.Instance;
		ec.OnContextChange += this.OnContextChange;
		_raycastAgent = _raycastObject.GetComponent<RaycastAgent> ();
	}

	public void Activate(int index) 
	{
		_moveToNewLocation (index);
		_toggleActivated (true);
	}

	public void Deactivate() 
	{
		_setPositionAndRotation(startingPosition, startingRotation);
		_toggleActivated (false);
	}

	public void SetInput(InputObject input) 
	{
		if (input.buttons ["cancel"]) 
		{
			EventCenter.Instance.ChangeContext(InputContext.PLAYER, "");
		} 
		else 
		{
			if (input.horizontal != 0 || input.vertical != 0) 
			{
				_move (input.horizontal, input.vertical);
				// Log ("PuzzleInspector/SetInput, horizontal = " + input.horizontal + ", vertical = " + input.vertical);
			}
		}
	}

	public void Log(string message)
	{
		if(isLogOn)
		{
			Debug.Log(message);
		}
	}
	#endregion

	#region private methods
	private void Awake () 
	{
		Init ();
		_toggleActivated (false);
	}

	private void Update() 
	{
//		Log ("PuzzleInspector/Update, isActive = " + this.isActive);
		if (this.isActive) 
		{
			_raycastAgent.CheckRayCast ();
		}
	}

	private int _getLocationIndex(string name) 
	{
		for (int i = 0; i < locations.Length; i++) 
		{
			if (locations [i].name == name) 
			{
				return i;
			}
		}
		return -1;
	}

	private void _moveToNewLocation(int index) 
	{
		PuzzleLocation location = locations [index];
		_setPositionAndRotation(location.position, location.rotation);

		_activeRotationY = location.rotation.y;
		_activeLocation = index;
		_activeBounds = location.bounds;
		_xDeviation = 0;
		_yDeviation = 0;

	}

	private void _setPositionAndRotation(Vector3 position, Vector3 rotation) 
	{
		// Log("PuzzleInspector/_setPositionAndRotaion, position = " + position + ", rotation = " + rotation);
		transform.position = new Vector3(position.x, position.y, position.z);
		transform.eulerAngles = new Vector3(rotation.x, rotation.y, rotation.z);
	}

	private void _toggleActivated(bool isActivated) 
	{
//		Log ("PuzzleInspector/_toggleActivated, isActivated = " + isActivated);
		EventCenter ec = EventCenter.Instance;

		if (isActivated) 
		{
			ec.OnNearItem += OnNearItem;
		} 
		else 
		{
			ec.OnNearItem -= OnNearItem;
		}

		if (this.isActive && !isActivated) 
		{
//			Log ("  was active, have to deactivate stuff");
			_raycastAgent.ClearFocus ();
			ec.InvokeStringEvent (Puzzle.ACTIVATE_EVENT);
		}
		_raycastObject.SetActive (isActivated);
		this.isActive = _camera.enabled = _light.enabled = isActivated;
	}

	private void _move(float horizontal, float vertical) 
	{
		float moveX = horizontal * _xSpeed;
		float moveY = vertical * _ySpeed;

		_xDeviation += moveX; 
		_yDeviation += moveY; 

		Vector3 newPosition = new Vector3(moveX, moveY, 0);
		_raycastObject.transform.Translate(newPosition);
	}

	private void OnDestroy() 
	{
		EventCenter ec = EventCenter.Instance;
		if (ec != null) 
		{
			ec.OnContextChange -= this.OnContextChange;
			ec.OnNearItem -= this.OnNearItem;
		}
	}
	#endregion
}

[Serializable]
public struct PuzzleLocation 
{
	public string name;
	public Vector3 position;
	public Vector3 rotation;
	public Bounds bounds;
}

[Serializable]
public struct Bounds 
{
	public float minH;
	public float maxH;
	public float minV;
	public float maxV;
}