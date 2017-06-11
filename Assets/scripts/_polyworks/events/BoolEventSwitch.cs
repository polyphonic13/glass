using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class BoolEventSwitch : Switch
	{
		public string type;
		public bool value = true; 

		public override void Actuate() {
			EventCenter.Instance.InvokeBoolEvent(type, value);
		}

		public override void Use () {
			Actuate ();
		}
	}
}

