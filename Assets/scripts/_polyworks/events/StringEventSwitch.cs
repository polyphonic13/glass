using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class StringEventSwitch : Switch
	{
		public string eventType; 

		public override void Actuate() {
//			Debug.Log ("StringEventSwitch/Actuate, name = " + this.name + ", eventType = " + eventType);
			EventCenter.Instance.InvokeStringEvent(eventType, this.name);
		}

		public override void Use() {
			Actuate ();
		}
	}
}

