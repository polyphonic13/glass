using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class BoolEventSwitch : EventSwitch
	{
		public bool value = true; 

		public override void Actuate() {
			EventCenter.Instance.InvokeBoolEvent(type, value);
		}
	}
}

