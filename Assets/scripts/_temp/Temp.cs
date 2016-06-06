using UnityEngine;
using System.Collections;
using Polyworks; 

public class Temp : MonoBehaviour {

	private Camera _mainCamera;
	private Camera _loadingCamera;

	public void Increment() {
		Game.Instance.Increment ();
	}

	public void Save() {
		Game.Instance.Save ();
	}

	public void Load() {
		Game.Instance.Load ();
	}
	
	public void ChangeScene(string scene) {
		Game.Instance.ChangeScene(scene);
	}

	public void OnSceneInitialized(string scene) {
		Debug.Log ("Temp/OnSceneInitialized");
		_updateCameras (true, false);
	}

	private void Awake() {

		_updateCameras (false, true);

		Polyworks.EventCenter.Instance.OnSceneInitialized += OnSceneInitialized;

	}

	private void _updateCameras(bool main, bool loading) {
		Debug.Log ("Temp/_updateCameras, main = " + main + ", loading = " + loading);
		GameObject mainCamera = GameObject.Find("main_camera");
		GameObject loadingCamera = GameObject.Find ("loading_camera");

		if (mainCamera != null) {
			_mainCamera = mainCamera.GetComponent<Camera>();
			_mainCamera.enabled = main;
		}

		if (loadingCamera != null) {
			_loadingCamera = loadingCamera.GetComponent<Camera>();
			_loadingCamera.enabled = loading;
		}
	}
}
