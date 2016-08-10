using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class StringEventSwitch : Switch
	{
		public int eventType; 

		public override void Actuate() {
			EventCenter.Instance.InvokeIntEvent(eventType, this.name);
		}
	}
}

