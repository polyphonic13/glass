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

	private string _activeLocation = "";

	private float _vertical; 
	private float _horizontal; 
	#endregion

	#region event handlers
	public void OnStringEvent(string type, string value) {
		if (type == Puzzle.ACTIVATE_EVENT) {
			Debug.Log ("PuzzleInspector/OnStringEvent, type = " + type + ", value = " + value);
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

	public void Activate(string name) {
		if (_activeLocation == "" || _activeLocation != name) {
			for (int i = 0; i < locations.Length; i++) {
				if (locations [i].name == name) {
					_setLocation(locations[i]);
					break;
				}
			}
		}
		_toggleActivated (true);
	}

	public void Deactivate() {
		_toggleActivated (false);
	}

	public void SetVertical(float vertical) {
		_vertical = vertical;
	}

	public void SetHorizontal(float horizontal) {
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

	private void _setLocation(PuzzleLocation location) {
		this.transform.position = new Vector3 (location.position.x, location.position.y, location.position.z);
		this.transform.Rotate(location.rotation);
		_activeLocation = location.name;
	}

	private void _toggleActivated(bool isActivated) {
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