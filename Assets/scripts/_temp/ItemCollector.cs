using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class ItemCollector : MonoBehaviour
	{
		public string collectKey; 

		private Item _item;

		void Start ()
		{
			_item = GetComponent<Item> ();
		}

		void Update ()
		{
			if (Input.GetKeyDown (collectKey)) {
				if (_item != null && _item.data.isCollectable && !_item.data.isCollected) {
					_item.Collect (Game.Instance.GetPlayerInventory());
					EventCenter.Instance.UpdateStringTask (_item.name, _item.name);

				}
			}
		}
	}

}
