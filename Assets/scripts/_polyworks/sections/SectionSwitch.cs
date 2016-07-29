using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class SectionSwitch : Switch
	{
		public string sectionName; 

		public override void Actuate() {
			Debug.Log ("SectionSwitch/Actuate, sectionName = " + sectionName);
			EventCenter.Instance.ChangeSection (sectionName);
		}
	}
}

