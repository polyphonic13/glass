using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class SectionSwitch : Switch
	{
		public string sectionName; 

		public override void Actuate() {
			EventCenter.Instance.ChangeSection (sectionName);
		}
	}
}

