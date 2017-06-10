using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class RaycastAgent : MonoBehaviour
	{
		public float detectionDistance = 4f;
		public string dynamicTag = "interactive";
		public string staticTag = "persistent"; 
		public bool isActive;

		public Color rayColor = Color.red;

		public ProximityAgent focusedItem { get; set; }
		public string itemJustHit { get; set; }

		public virtual void CheckRayCast() {
//			Debug.Log ("RaycastAgent[" + this.name + "]/CheckRayCast, dynamicTag = " + dynamicTag);
			RaycastHit hit;
			if (Physics.Raycast (this.transform.position, this.transform.forward, out hit, detectionDistance)) {
				Debug.DrawRay (this.transform.position, this.transform.forward, rayColor);
//				Debug.Log (" hit tag = " + hit.transform.tag + ", name = " + hit.transform.name);
				if (hit.transform != this.transform && (hit.transform.tag == dynamicTag || hit.transform.tag == staticTag)) {
//					Debug.Log (" hit name = " + hit.transform.name + ", just hit = " + itemJustHit);
					if (hit.transform.name != itemJustHit) {
						ProximityAgent pa = hit.transform.gameObject.GetComponent<ProximityAgent> ();
//						Debug.Log ("  pa = " + pa);
						if (pa != null) {
							if (pa.Check ()) {
								pa.SetFocus (true);
								itemJustHit = hit.transform.name;
								focusedItem = pa;
							}
						}
					}
				} else {
					_clearFocus();
				}
			} else {
				_clearFocus();
			}
		}

		public void ClearFocus() {
			Debug.Log ("RaycastAgent/ClearFocus, focusedItem = " + focusedItem);
			_clearFocus ();
		}

		private void _clearFocus() {
			if(focusedItem != null) {
				focusedItem.SetFocus (false);
				focusedItem = null;
			}
			itemJustHit = "";
		}
	}
}

