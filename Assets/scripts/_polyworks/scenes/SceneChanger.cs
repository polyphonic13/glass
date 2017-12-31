using UnityEngine;
using System;
using System.Collections;

namespace Polyworks {
	[Serializable]
	public struct ScenePrepSteps {
		public string scene;
		public int count;
	}

	public class SceneChanger : Singleton<SceneChanger>
	{
		#region public members
		public ScenePrepSteps[] prepSteps;
		#endregion

		#region private members
		private string _currentScene = "";
		private string _targetScene = "";
		private int _targetSection = -1;

		private ScenePrepSteps _currentPrepSteps;
		private int _totalPrepStepsCompleted = 0;
		private bool _isListenersAdded = false;
		#endregion

		#region public methods
		public void Init(string scene) {
			_currentScene = scene;
			for (int i = 0; i < prepSteps.Length; i++) {
				if (prepSteps [i].scene == scene) {
					_currentPrepSteps = prepSteps [i];
					break;
				}
			}
			if (!_isListenersAdded) {
				EventCenter ec = EventCenter.Instance;
				ec.OnStartSceneChange += OnStartSceneChange;
				ec.OnContinueSceneChange += OnContinueSceneChange;
				_isListenersAdded = true;
			}
		}

		public void OnStartSceneChange(string scene, int section = -1) {
			_targetScene = scene;
			_targetSection = section;
		}

		public void OnContinueSceneChange(string scene, int section = -1) {
			_totalPrepStepsCompleted++; 
			Debug.Log ("SceneChanger/OnContinueSceneChange, _totalPrepStepsCompleted = " + _totalPrepStepsCompleted + ", _currentPrepSteps.count = " + _currentPrepSteps.count);
			if (_totalPrepStepsCompleted == _currentPrepSteps.count) {
				_totalPrepStepsCompleted = 0;
				Debug.Log (" dispatching CompleteSceneChange");
				EventCenter.Instance.CompleteSceneChange(_targetScene, _targetSection);
			}
		}
		#endregion

		private void OnDestroy() {
			EventCenter ec = EventCenter.Instance;
			if (ec != null) {
				ec.OnStartSceneChange -= OnStartSceneChange;
				ec.OnContinueSceneChange -= OnStartSceneChange;
			}
		}
	}

}


