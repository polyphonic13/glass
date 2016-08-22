using UnityEngine;
using System.Collections;
using Polyworks;

public class IntEventAgent : MonoBehaviour {
	public static string EVENT_VALUE = -1;

	public override void Use() {
		EventCenter.Instance.InvokeIntEvent(EVENT_VALUE, this.name);
	}
}
