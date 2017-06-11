﻿namespace Polyworks {
	using UnityEngine;
	using System.Collections;

	public class Switch : MonoBehaviour
	{
		public virtual void Actuate() {
			Debug.Log ("Switch["+this.name+"]/Actuate");
		}

		public virtual void Use() {
			Debug.Log ("Switch[" + this.name + "]/Use");
		}
	}
}

