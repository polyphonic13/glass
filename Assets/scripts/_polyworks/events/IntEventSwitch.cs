using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class IntEventSwitch : Switch
	{
		public int value; 

		public override void Actuate() {
			EventCenter.Instance.InvokeIntEvent(this.name, value);
		}
	}
}

