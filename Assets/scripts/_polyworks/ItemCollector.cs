using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class ItemCollector : MonoBehaviour
	{

		private Item _item;
		// Use this for initialization
		void Start ()
		{
			_item = GetComponent<Item> ();
		}

		// Update is called once per frame
		void Update ()
		{
			if (Input.GetKeyDown ("m")) {
				Debug.Log ("m down");
				if (_item != null && _item.Data.isCollectable && !_item.Data.isCollected) {
					_item.Collect (Inventory.Instance);
				}
			}
		}
	}

}
