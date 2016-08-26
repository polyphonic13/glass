using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class UsableRangeAgent : MonoBehaviour
	{
		public string targetName1;
		public string targetName2 = "player";
		public float usableDistance = 2f;

		private Transform _target1;
		private Transform _target2;

		private Item _item;

		private bool _isCollected = false;

		public void Collect() {
//			Debug.Log ("UsableRangeAgent[" + this.name + "]/Collect");
			GameObject object1 = GameObject.Find (targetName1);
			GameObject object2 = GameObject.Find (targetName2);

			if (object1 != null) {
				_target1 = object1.transform;
			}

			if (object2 != null) {
				_target2 = object2.transform;
			}

			Debug.Log ("UsableRangeAgent[" + this.name + "]/Collect, _target1 = " + _target1 + ", _target2 = " + _target2);
		}

		void Awake () {
			_item = GetComponent<Item> ();
		}
			
		void Update () {
			if( _target1 != null && _target2 != null) {
				Debug.Log ("enabled = " + _item.isEnabled + ", collected = " + _item.data.isCollected);
				if (_item.isEnabled && _item.data.isCollected) {
					var difference = Vector3.Distance(_target1.position, _target2.position);
					Debug.Log ("UsableRangeAgent/Update, difference = " + difference + ", usableDifference = " + usableDistance);
					if(difference < usableDistance) {
						if(!_item.data.isUsable) {
							Debug.Log (this.name + " is enabled, proximity difference = " + difference);
							_item.data.isUsable = true;
						}
					} else if(_item.data.isUsable) {
						_item.data.isUsable = false;
					}
				}
			}
		}
	}
}

