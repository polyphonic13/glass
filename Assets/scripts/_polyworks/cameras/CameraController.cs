using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class CameraController : MonoBehaviour
	{
		public bool isMain;

		private Camera _camera;

		public void OnSceneInitialized(string scene) {
//			Debug.Log ("CameraController/OnSceneInitialized");
			if (_camera != null) {
				_camera.enabled = true;
				EventCenter.Instance.MainCameraEnabled ();
			}
		}

		public void OnChangeScene(string scene, int section) {
//			Debug.Log ("CameraController/OnChangeScene, _camera = " + _camera);
			if (_camera != null) {
				_camera.enabled = false;
			}
		}

		private void Awake() {
			if (isMain) {
				_camera = Camera.main;
				_camera.enabled = false;

				var ec = EventCenter.Instance;
				if (ec != null) {
					ec.OnSceneInitialized += OnSceneInitialized;
					ec.OnChangeScene += OnChangeScene;
				}
			} else {
				_camera = GetComponent<Camera> ();
			}
		}

		private void OnDestroy() {
			if (isMain) {
				var ec = EventCenter.Instance;
				if (ec != null) {
					ec.OnSceneInitialized -= OnSceneInitialized;
					ec.OnChangeScene -= OnChangeScene;
				}
			}
		}

	}
}


