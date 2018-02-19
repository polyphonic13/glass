namespace Polyworks {
	using UnityEngine;
	using System.Collections;

	public class DeactivateGameObjectSwitch : Switch
	{
		public bool isTargetSelf;

		public GameObject target; 

		public override void Actuate ()
		{
			Log ("DeactivateGameObjectSwitch[" + this.name + "]/Actuate, target = " + target);
			base.Actuate ();
			if (target != null) {
				target.SetActive (false);
			}
		}

		private void Awake() {
			if(isTargetSelf) {
				target = this.gameObject;
			}
		}
	}
}