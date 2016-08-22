using UnityEngine;
using System.Collections;
using Polyworks;

public class StringEventAgent : MonoBehaviour {
	public static string value = "";

	public override void Use() {
		EventCenter.Instance.InvokeStringEvent(value, this.name);
	}
}
