namespace Polyworks {
	using UnityEngine;

	public class ActuateAgent: MonoBehaviour, IActuatable {
		public bool isLogOn = false; 

		public virtual void Actuate() {
//			Debug.Log ("ActuateAgent[" + this.name + "]/Actuate");
		}
		public virtual void Use() {}

		public virtual void Log(string message) {
			if (isLogOn) {
				Debug.Log (message);
			}
		}
	}
}