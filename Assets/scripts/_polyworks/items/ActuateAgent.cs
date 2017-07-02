namespace Polyworks {
	using UnityEngine;

	public class ActuateAgent: MonoBehaviour, IActuatable {
		public virtual void Actuate() {}
		public virtual void Use() {}
	}
}