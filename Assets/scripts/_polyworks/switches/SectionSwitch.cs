using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class SectionSwitch : Switch
	{
		public int section; 

		public override void Actuate() {
			EventCenter.Instance.ChangeSection (section);
		}
	}
}

