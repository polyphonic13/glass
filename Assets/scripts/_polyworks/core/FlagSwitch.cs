namespace Polyworks {
	using UnityEngine;

	public class FlagSwitch: Switch {

		public string key;
		public bool isActivate = true; 

		public override void Use() {
			Debug.Log ("FlagSwitch[" + this.name + "]/Use, key = " + key);

			Game.Instance.SetFlag (key, isActivate); 

			isActivate = !isActivate;
		}
	}
}
