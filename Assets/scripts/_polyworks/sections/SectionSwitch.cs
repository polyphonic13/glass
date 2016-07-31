using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class SectionSwitch : Switch
	{
		public int section; 

		public override void Actuate() {
//			Debug.Log ("SectionSwitch/Actuate, sectionName = " + section);
			EventCenter.Instance.ChangeSection (section);
		}
	}
}

