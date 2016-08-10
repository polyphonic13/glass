using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class StringEventSwitch : Switch
	{
		public string eventType; 

		public override void Actuate() {
			EventCenter.Instance.InvokeStringEvent(eventType, this.name);
		}
	}
}

