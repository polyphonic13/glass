using UnityEngine;
using System.Collections;

public class PlayerHead : MonoBehaviour {

	public float interactDistance = 2f;

	private InteractiveItem _focusedItem;
	private string _itemJustHit;

	void Update () {
		_checkRayCast();
	}

	private void _checkRayCast() {
		RaycastHit hit;

		if (Physics.Raycast (this.transform.position, this.transform.forward, out hit, interactDistance)) {
			if (hit.transform != this.transform && hit.transform.tag == "interactive") {
				if (hit.transform.name != _itemJustHit) {
					Debug.DrawRay (this.transform.position, this.transform.forward, Color.red);
					InteractiveItem item = hit.transform.gameObject.GetComponent<InteractiveItem> ();
					item.SetFocus (true);
					_itemJustHit = hit.transform.name;
					_focusedItem = item;
				}
			}
		} else {
			if(_focusedItem != null) {
				_focusedItem.SetFocus (false);
				_focusedItem = null;
			}
			_itemJustHit = "";
		}
	}
}
