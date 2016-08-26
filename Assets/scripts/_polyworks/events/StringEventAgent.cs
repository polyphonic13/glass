using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class StringEventAgent : MonoBehaviour {
		public static string value = "";

		public void Use() {
			EventCenter.Instance.InvokeStringEvent(value, this.name);
		}
	}
}
