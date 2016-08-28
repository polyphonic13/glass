using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class DestroySwitch : Switch
	{
		public string target = "";

		public override void Actuate ()
		{
			if (target != "") {
				EventCenter.Instance.InvokeDestroy (target);
			}
		}
	}
}

