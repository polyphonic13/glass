namespace Polyworks {
	using UnityEngine;
	using System.Collections;

	public class ActuateTargetAgent : Item
	{
		public ActuateAgent target;

		public override void Actuate ()
		{
//			Debug.Log ("ActuateTargetAgent[" + this.name + "]/Actuate, target = " + target);
			target.Actuate ();
		}

		public override void Use ()
		{
			target.Use ();
		}
	}
}