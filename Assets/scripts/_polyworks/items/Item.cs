using UnityEngine;
using System.Collections;

namespace Polyworks {

	public class Item : MonoBehaviour
	{
		public ItemData data;

		public bool isEnabled { get; set; }

		public int icon;

		public virtual void Actuate(Inventory inventory) {
			// Debug.Log ("Item[" + this.name + "]/Actuate");
		}

		public virtual void Use() {}

		public void SetData(ItemData d) {
			data = d;
		}
	}
}

