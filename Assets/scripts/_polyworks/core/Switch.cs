namespace Polyworks {
	using UnityEngine;
	using System.Collections;

	public class Switch : ActuateAgent
	{
		public virtual void Actuate() {
			Log ("Switch["+this.name+"]/Actuate");
		}

		public virtual void Use() {
			Log ("Switch[" + this.name + "]/Use");
		}
	}
}

