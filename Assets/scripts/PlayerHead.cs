using UnityEngine;
using System.Collections;

public class PlayerHead : MonoBehaviour {

	public float interactDistance = 2f;

	private InteractiveItem _focusedItem;
	private string _itemJustHit;

	void Update () {
		_checkRayCast();
	}

	private void _clearFocus() {
		if(_focusedItem != null) {
			_focusedItem.SetFocus (false);
			_focusedItem = null;
		}
		_itemJustHit = "";

	}
	
	private void _checkRayCast() {
		RaycastHit hit;

		if (Physics.Raycast (this.transform.position, this.transform.forward, out hit, interactDistance)) {
//			Debug.DrawRay(this.transform.position, this.transform.forward, Color.green);
			if (hit.transform != this.transform && (hit.transform.tag == "interactive")) {
				if (hit.transform.name != _itemJustHit) {
					InteractiveItem item = hit.transform.gameObject.GetComponent<InteractiveItem> ();
					if(item.IsEnabled) {
						item.SetFocus (true);
						_itemJustHit = hit.transform.name;
						_focusedItem = item;
					}
				}
			} else {
				_clearFocus();
			}
		} else {
			_clearFocus();
		}
	}
	
}
