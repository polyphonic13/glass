using UnityEngine;

namespace Polyworks {
	public class ProximityAgent : MonoBehaviour {

		public float interactDistance = 2f;
		public Transform target;
		public bool isTargetPlayer = true; 

		private bool _wasJustFocused;
		private bool _isInitialized;
		private Item _item;
	
	
		public void SetFocus(bool isFocused) {
			Debug.Log ("ProximityAgent[" + this.name + "]/SetFocus, isFocused = " + isFocused + ", item = " + _item);
			if(_isInitialized && _item.isEnabled) {
				if (isFocused) {
					if (!_wasJustFocused) {
						EventCenter.Instance.ChangeItemProximity(_item, true);
						_wasJustFocused = true;
					}
				} else if (_wasJustFocused) {
					EventCenter.Instance.ChangeItemProximity(_item, false);
					_wasJustFocused = false;
				}
			}
		}

		public bool Check() {
			bool isInProximity = false;
			if (_item.isEnabled) {
				var difference = Vector3.Distance (target.position, transform.position);
				if (difference < interactDistance) {
					isInProximity = true;
					EventCenter.Instance.ChangeItemProximity (_item, isInProximity);
					_wasJustFocused = true;
				} else if (_wasJustFocused) {
					EventCenter.Instance.ChangeItemProximity (_item, isInProximity);
					_wasJustFocused = false;
				}
			}
			return isInProximity;
		}

		public void OnSceneInitialized(string scene) {
			Init();
		}

		public void Init() {
			if(!_isInitialized) {
				_item = gameObject.GetComponent<Item> ();
				if (isTargetPlayer) {
					target = GameObject.Find ("player").transform;
				}
//				Debug.Log ("ProximityAgent[" + this.name + "]/Init, target = " + target);
				_isInitialized = true;
			}
		}
		
		private void Awake() {
//			Debug.Log ("ProximityAgent[" + this.name + "]/Awake");
			EventCenter.Instance.OnSceneInitialized += this.OnSceneInitialized;
		}

		private void OnDestroy() {
			EventCenter ec = EventCenter.Instance;
			if (ec != null) {
				ec.OnSceneInitialized -= this.OnSceneInitialized;
			}
		}
	}
}
