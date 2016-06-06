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
		_mainCamera = GameObject.Find("main_camera").GetComponent<Camera>();
		_loadingCamera = GameObject.Find ("loading_camera").GetComponent<Camera>();

		if (_mainCamera != null) {
			_mainCamera.enabled = true;
		}

		if (_loadingCamera != null) {
			_loadingCamera.enabled = false;
		}
	}

	void Awake() {
		_mainCamera = GameObject.Find("main_camera").GetComponent<Camera>();
		_loadingCamera = GameObject.Find ("loading_camera").GetComponent<Camera>();

		if (_mainCamera != null) {
			_mainCamera.enabled = false;
		}

		if (_loadingCamera != null) {
			_loadingCamera.enabled = true;
		}

		Polyworks.EventCenter.Instance.OnSceneInitialized += OnSceneInitialized;

	}
}
