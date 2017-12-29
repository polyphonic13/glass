namespace Polyworks {
	using UnityEngine;
	using System.Collections;

	public class EventSwitch : Switch
	{
		public string type;

		public override void Actuate() {
			Debug.Log ("EventSwitch[" + this.name + "]/Actuate, type = " + type);
		}

		public override void Use() {
			Actuate ();
		}
	}
}