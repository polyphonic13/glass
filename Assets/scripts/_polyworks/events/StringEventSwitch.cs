using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class StringEventSwitch : EventSwitch
	{
		public override void Actuate() {
			Log ("StringEventSwitch["+this.name+"]/Actuate, type = " + type + ", value = " + this.name);
			EventCenter.Instance.InvokeStringEvent(type, this.name);
		}
	}
}

