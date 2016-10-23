using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class SceneChanger : Singleton<SceneChanger>
	{
		#region delegates
		public delegate void Changer(string scene, int section);
		public event Changer OnSceneChangePrep;
		public event Changer OnSceneChange;
		#endregion

		#region public members
		public int totalPrepSteps = 0;
		#endregion

		#region private members
		private string _targetScene = "";
		private int _targetSection = -1;

		private int _totalPrepStepsCompleted = 0;
		#endregion

		#region public methods
		public void Execute(string scene, int section = -1) {
			Debug.Log ("SceneChanger/Execute, scene = " + scene + ", section = " + section);
			_targetScene = scene;
			_targetSection = section;

			if (OnSceneChangePrep != null) {
				OnSceneChangePrep (scene, section);
			}
		}

		public void Continue() {
			_totalPrepStepsCompleted++; 
			Debug.Log ("SceneChanger/Continue, _totalPrepStepsCompleted = " + _totalPrepStepsCompleted + ", totalPrepSteps = " + totalPrepSteps);
			if (_totalPrepStepsCompleted == totalPrepSteps) {
				_totalPrepStepsCompleted = 0;
				Debug.Log (" all prep done, time to change the scene");
				if (OnSceneChange != null) {
					OnSceneChange (_targetScene, _targetSection);
				}
			}
		}
		#endregion
	}
}

