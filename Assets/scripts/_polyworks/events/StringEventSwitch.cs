using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class StringEventSwitch : Switch
	{
		public string value; 

		public override void Actuate() {
			EventCenter.Instance.InvokeStringEvent(this.name, value);
		}
	}
}

