﻿using UnityEngine;
using System.Collections;

namespace Polyworks {

	public class Item : MonoBehaviour
	{
		public bool IsEnabled { get; set; }

		public ItemData Data;

		public void Collect(Inventory inventory) {
			if (Data.isCollectable && !Data.isCollected) {
				inventory.Add (Data.Clone ());
				GameObject.Destroy (gameObject);
			}
		}

		public void SetData(ItemData data) {
			Data = data;
		}
	}
}

