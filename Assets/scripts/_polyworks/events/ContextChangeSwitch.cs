using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class ContextChangeSwitch : Switch
	{
		public InputContext contextType; 

		public override void Actuate() {
			Debug.Log ("ContextChangeSwitch/Actuate, name = " + this.name + ", eventType = " + contextType);
			EventCenter.Instance.ChangeContext(contextType, this.name);
		}

		public override void Use() {
			Actuate ();
		}
	}
}
