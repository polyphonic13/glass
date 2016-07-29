using UnityEngine;

namespace Polyworks {
	public class ProximityController : MonoBehaviour {

		public float interactDistance = 2f;
		public Transform target;
		public bool isTargetPlayer; 

		private bool _wasJustFocused;
		private bool _isInitialized;
		private Item _item;
	
	
		public void SetFocus(bool isFocused) {
			// Debug.Log ("ProximityController[" + this.name + "]/SetFocus, isFocused = " + isFocused + ", item = " + _item);
			if(_isInitialized) {
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
			if (_item.enabled) {
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
				_isInitialized = true;
			}
		}
		
		private void Awake() {
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
