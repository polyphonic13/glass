using UnityEngine;
using System.Collections;

namespace Polyworks {

	[System.Serializable]
	public class GoalController : MonoBehaviour
	{
		public Goal[] goals;

		public void Init() {

		}

		public void OnGoalCompleted(string name) {
			for (int i = 0; i < goals.Length; i++) {
				if (goals [i].name == name) {
					goals [i].isCompleted = true;
				}
			}
		}

	}

	[System.Serializable]
	public class Goal {
		public string name;
		public bool isCompleted;

		public enum GoalType { COUNT, VALUE, STRING }
		public GoalType type;
	}
}
