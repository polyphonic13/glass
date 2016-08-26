using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class ItemCollector : MonoBehaviour
	{
		public string collectKey; 

		private CollectableItem _item;

		void Start ()
		{
			_item = GetComponent<CollectableItem> ();
		}

		void Update ()
		{
			if (Input.GetKeyDown (collectKey)) {
				if (_item != null && !_item.data.isCollected) {
					_item.Actuate ();
					EventCenter.Instance.UpdateStringTask (_item.name, _item.name);

				}
			}
		}
	}

}
