﻿using UnityEngine;
using System.Collections;

namespace Polyworks {

	public class Item : MonoBehaviour
	{
		public ItemData data;

		public bool isEnabled { get; set; }

		public int icon = 1;

		public virtual void Actuate() {
			// Debug.Log ("Item[" + this.name + "]/Actuate");
		}

		public virtual void Use() {}

		public void SetData(ItemData d) {
			data = d;
		}

		public virtual void SetEnabled(bool isEnabled) {
			this.isEnabled = isEnabled;
		}

		public virtual void Enable() {
			this.SetEnabled (true);
		}

		public virtual void Disable() {
			this.SetEnabled (false);
		}
	}
}

