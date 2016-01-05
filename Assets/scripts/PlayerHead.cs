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

		Debug.DrawRay(this.transform.position, this.transform.forward, Color.red);
		if (Physics.Raycast (this.transform.position, this.transform.forward, out hit, interactDistance)) {
			if (hit.transform != this.transform && (hit.transform.tag == "interactive" || hit.transform.tag == "persistent")) {
				Debug.DrawRay(this.transform.position, this.transform.forward, Color.green);
//				Debug.Log("hit name = " + hit.transform.name);
				if (hit.transform.name != _itemJustHit) {
					InteractiveItem item = hit.transform.gameObject.GetComponent<InteractiveItem> ();
					if(item.IsEnabled) {
//						bool isLookingAtItem = IsLookingAtObject(this.transform, item.transform);
//						Debug.Log ("isLookingAtItem[ " + item.name + " ] = " + isLookingAtItem);
//						if(isLookingAtItem) {
							item.SetFocus (true);
							_itemJustHit = hit.transform.name;
							_focusedItem = item;
//						} else {
//							_clearFocus();
//						}
					}
				}
			} else {
				_clearFocus();
			}
		} else {
			_clearFocus();
		}
	}
	
	bool IsLookingAtObject(Transform obj, Transform targetObj) {
		//direction of the target as a vector
		Vector3 direction = targetObj.position - obj.position;
		
		float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		
		//current angle in degrees
		float anglecurrent = obj.eulerAngles.z;
		float checkAngle = 0;
		
		//Checking to see what quadrant current angle is at
		if (anglecurrent > 0 && anglecurrent <= 90) {
			checkAngle = ang - 90;
		}
		if (anglecurrent > 90 && anglecurrent <= 360) {
			checkAngle = ang + 270;
		}
		
		//If current angle is equal to the angle that I need to be at to look at the object, return true
		//It is possible to do "if (checkAngle == anglecurrent)" but some times you don't get an exact match so do something like below to have some error range.
		
		if (anglecurrent <= checkAngle + 0.5f && anglecurrent >= checkAngle - 0.5f) {
			return true;  
		} else {
			return false;
		}
	}
}
