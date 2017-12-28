namespace Polyworks {
	using UnityEngine;

	public class ActuateAgent: MonoBehaviour, IActuatable {
		public virtual void Actuate() {
			Debug.Log ("ActuateAgent[" + this.name + "]/Actuate");
		}
		public virtual void Use() {}
	}
}