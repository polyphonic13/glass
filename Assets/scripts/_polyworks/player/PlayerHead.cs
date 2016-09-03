using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class PlayerHead : MonoBehaviour {

		public float interactDistance = 2f;

		private ProximityAgent _focusedItem;
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
				Debug.Log (" hit tag = " + hit.transform.tag + ", name = " + hit.transform.name);
				if (hit.transform != this.transform && (hit.transform.tag == "interactive" || hit.transform.tag == "persistent")) {
					Debug.Log (" hit name = " + hit.transform.name + ", just hit = " + _itemJustHit);
					if (hit.transform.name != _itemJustHit) {
						ProximityAgent pa = hit.transform.gameObject.GetComponent<ProximityAgent> ();
						Debug.Log ("  pa = " + pa);
						if(pa != null) {
							pa.SetFocus (true);
							_itemJustHit = hit.transform.name;
							_focusedItem = pa;
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
}
