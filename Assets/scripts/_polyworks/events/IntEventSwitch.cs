using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class IntEventSwitch : EventSwitch
	{
		public int value; 

		public override void Actuate() {
			Debug.Log ("IntEventSwitch[" + this.name + "]/Actuate, type = " + type + ", value = " + value);
			EventCenter.Instance.InvokeIntEvent(type, value);
		}
	}
}

