using UnityEngine;
using System.Collections;
using System;

public class PuzzleInspector : MonoBehaviour {

	public PuzzleLocation[] locations;

	public Camera camera;
	public Light light;

	private string _activeLocation = "";

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

	void Awake () {
		_toggleActivated (false);
	}

	private void _setLocation(PuzzleLocation location) {
		this.transform.position = new Vector3 (location.position.x, location.position.y, location.position.z);
		this.transform.Rotate(location.rotation);
		_activeLocation = location.name;
	}

	private void _toggleActivated(bool isActivated) {
		this.camera.enabled = isActivated; 
		this.light.enabled = isActivated;
	}
}

[Serializable]
public struct PuzzleLocation {
	public string name;
	public Vector3 position;
	public Vector3 rotation;
}