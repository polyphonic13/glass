﻿using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class Switch : MonoBehaviour
	{
		public virtual void Actuate() {
			Debug.Log ("Switch["+this.name+"]/Acutate");
		}
	}
}

