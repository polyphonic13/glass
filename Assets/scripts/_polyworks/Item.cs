using UnityEngine;
using System.Collections;

namespace Polyworks {

	public class Item : MonoBehaviour
	{
		public bool IsEnabled { get; set; }

		public ItemData data;

		public void Collect(Inventory inventory) {
			if (data.isCollectable && !data.isCollected) {
				inventory.Add (data.Clone ());
				GameObject.Destroy (gameObject);
			}
		}

		public void SetData(ItemData d) {
			data = d;
		}
	}
}

