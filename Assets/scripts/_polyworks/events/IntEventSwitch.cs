using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class IntEventSwitch : Switch
	{
		public string type;
		public int value; 

		public override void Actuate() {
			EventCenter.Instance.InvokeIntEvent(type, value);
		}

		public override void Use () {
			Actuate ();
		}
	}
}

