using UnityEngine;
using System.Collections;
using System;
using UnitySampleAssets.Characters.FirstPerson;
using Polyworks; 

public class PuzzleInspector : MonoBehaviour, IInputControllable {

	#region members
	public PuzzleLocation[] locations;

	public bool isActive { get; set; }

	[SerializeField] private Light _light;
	[SerializeField] private Camera _camera;
	[SerializeField] private MouseLook _mouseLook;

	private int _activeLocation = -1;

	private float _vertical; 
	private float _horizontal; 
	#endregion

	#region event handlers
	public void OnStringEvent(string type, string value) {
		if (type == Puzzle.ACTIVATE_EVENT) {
			string target = value.Substring (0, 8);
			Debug.Log ("PuzzleInspector/OnStringEvent, type = " + type + ", value = " + value + ", target = " + target);
			int index = _getLocationIndex (target);
			if (index > -1) {
				Debug.Log (" has location, going to activate with index: " + index);
				Activate (index);
			}
		}
	}

	public void OnActivatePuzzle(string name, bool isActive) {
		if (isActive) {

		} else {

		}
	}

	#endregion

	#region public methods
	public void Init() {
		EventCenter ec = EventCenter.Instance;
		ec.OnStringEvent += this.OnStringEvent;
		ec.OnActivatePuzzle += this.OnActivatePuzzle; 

		_mouseLook.Init (this.transform, _camera.transform);
	}

	public void Activate(int index) {
		if (index != _activeLocation) {
			// move the inspector to new location
			_setLocation (index);
		}
		_toggleActivated (true);
		EventCenter.Instance.ChangeContext(InputContext.PUZZLE);
	}

	public void Deactivate() {
		_toggleActivated (false);
	}

	public void SetInput(InputObject input) {
		if (input.buttons ["cancel"]) {
			_toggleActivated (false);
			EventCenter.Instance.ChangeContext(InputContext.PLAYER);
		} else {
			// handle the input
		}
	}

	public void SetVertical(float vertical) {
		Debug.Log ("PuzzleInspector/SetVertical, vertical =  " + vertical);
		_vertical = vertical;
	}

	public void SetHorizontal(float horizontal) {
		Debug.Log ("PuzzleInspector/SetVertical, horizontal =  " + horizontal);
		_horizontal = horizontal;
	}
	#endregion

	#region private methods
	private void Awake () {
		_toggleActivated (false);
		Init ();
	}

	private void Update() {
		if (this.isActive) {

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
		_activeLocation = index;
	}

	private void _toggleActivated(bool isActivated) {
		Debug.Log ("PuzzleInspector/_toggleActivated, isActivated = " + isActivated);
		this._camera.enabled = isActivated; 
		this._light.enabled = isActivated;
	}

	private void OnDestroy() {
		EventCenter ec = EventCenter.Instance;
		if (ec != null) {
			ec.OnStringEvent -= this.OnStringEvent;
			ec.OnActivatePuzzle -= this.OnActivatePuzzle; 
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