using UnityEngine;
using System.Collections;
using Polyworks;

public class StringEventAgent : MonoBehaviour {
	public static string EVENT_NAME = "";

	public override void Use() {
		EventCenter.Instance.InvokeStringEvent(EVENT_NAME, this.name);
	}
}
