namespace Polyworks {
	using UnityEngine;
	using System.Collections;

	public class DeactivateGameObjectSwitch : Switch
	{
		public bool isTargetSelf;

		public GameObject target; 

		public override void Actuate ()
		{
			Debug.Log ("DeactivateGameObjectSwitch[" + this.name + "]/Actuate, target = " + target);
			base.Actuate ();
			if (target != null) {
				target.SetActive (false);
			}
		}
	}
}