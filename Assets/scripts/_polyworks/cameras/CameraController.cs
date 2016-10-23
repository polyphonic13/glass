using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class CameraController : MonoBehaviour
	{
		public bool isMain = false;
		public bool isInterScene = false; 

		private Camera _camera;

		public void OnSceneInitialized(string scene) {
			if (_camera != null) {
				if (isInterScene) {
					_camera.enabled = false;
				} else {
					_camera.enabled = true;
					if (isMain) {
						EventCenter.Instance.MainCameraEnabled ();
					}
				}
			}
		}

		public void OnChangeScenePrep(string scene, int section) {
			Debug.Log ("CameraController/OnChangeScenePrep, _camera = " + _camera + ", isMain = " + isMain + ", isInterScene = " + isInterScene);
			if (_camera != null) {
				if (isInterScene) {
					_camera.enabled = true;
				} else {
					_camera.enabled = false;
				}
			}
			SceneChanger.Instance.Continue ();

		}

		private void Awake() {
			Debug.Log ("CameraController/Awake, isMain = " + isMain);
			_camera = GetComponent<Camera> ();

			if (isInterScene) {
				_camera.enabled = true;
			} else {
				_camera.enabled = false;
			}

			var ec = EventCenter.Instance;
			if (ec != null) {
				ec.OnSceneInitialized += OnSceneInitialized;
			}
			var sc = SceneChanger.Instance; 
			if (sc != null) {
				sc.OnSceneChangePrep += OnChangeScenePrep;
			}
			Debug.Log ("  _camera = " + _camera);
		}

		private void OnDestroy() {
			if (isMain) {
				var ec = EventCenter.Instance;
				if (ec != null) {
					ec.OnSceneInitialized -= OnSceneInitialized;
				}
				var sc = SceneChanger.Instance; 
				if (sc != null) {
					sc.OnSceneChangePrep -= OnChangeScenePrep;
				}
			}
		}

	}
}


