using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class StringEventSwitch : Switch
	{
		public string value; 

		public override void Actuate() {
//			Debug.Log ("StringEventSwitch/Actuate, name = " + this.name + ", value = " + value);
			EventCenter.Instance.InvokeStringEvent(this.name, value);
		}
	}
}

