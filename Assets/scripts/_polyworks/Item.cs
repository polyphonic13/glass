using UnityEngine;
using System.Collections;

namespace Polyworks {

	public class Item : MonoBehaviour
	{
		public ItemData data;

		public bool isEnabled { get; set; }

		public int icon;

		public virtual void Collect(Inventory inventory) {
			if (data.isCollectable && !data.isCollected) {
				data.isCollected = true;
				inventory.Add (data.Clone ());
				GameObject.Destroy (gameObject);
			}
		}

		public virtual void Actuate() {}

		public virtual void Use() {}

		public void SetData(ItemData d) {
			data = d;
		}
	}
}

