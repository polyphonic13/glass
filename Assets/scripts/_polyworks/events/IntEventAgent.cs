using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class IntEventAgent : MonoBehaviour {
		public static int value = -1;

		public void Use() {
			EventCenter.Instance.InvokeIntEvent(this.name, value);
		}
	}
}
