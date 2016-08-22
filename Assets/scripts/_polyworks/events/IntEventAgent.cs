using UnityEngine;
using System.Collections;
using Polyworks;

public class IntEventAgent : MonoBehaviour {
	public static string value = -1;

	public override void Use() {
		EventCenter.Instance.InvokeIntEvent(value, this.name);
	}
}
