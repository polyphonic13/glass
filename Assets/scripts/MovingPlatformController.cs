﻿using UnityEngine;
using System.Collections;

public class MovingPlatformController : TargetController {

	[SerializeField] MovingPlatform target;

	public override void Actuate() {
		Debug.Log ("MovingPlatformController/Actuate, is active = " + target.GetIsActive ());
		if (!target.GetIsActive ()) {
			target.Actuate ();
		}
	}

	public override bool GetIsActive() {
		return target.GetIsActive ();
	}

}
