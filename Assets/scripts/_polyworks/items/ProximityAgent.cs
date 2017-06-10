using UnityEngine;

namespace Polyworks {
	public class ProximityAgent : MonoBehaviour {

		public float interactDistance = 2f;
		public Transform target;
		public bool isTargetPlayer = true; 

		private bool _wasJustFocused;
		private bool _isInitialized;
		private Item _item;
	
		#region handlers
		public void OnSceneInitialized(string scene) {
			Init();
		}
		#endregion

		#region public methods
		public void SetFocus(bool isFocused) {
//			Debug.Log ("ProximityAgent[" + this.name + "]/SetFocus, isFocused = " + isFocused + ", _isInitialized = " + _isInitialized + ", isEnabled = " + _item.isEnabled);
			SendMessage ("SetInProximity", isFocused, SendMessageOptions.DontRequireReceiver);
			if(_isInitialized && _item.isEnabled) {
				if (isFocused) {
					if (!_wasJustFocused) {
						EventCenter.Instance.NearItem(_item, true);
						_wasJustFocused = true;
					}
				} else if (_wasJustFocused) {
					EventCenter.Instance.NearItem(_item, false);
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
					EventCenter.Instance.NearItem (_item, isInProximity);
					_wasJustFocused = true;
				} else if (_wasJustFocused) {
					EventCenter.Instance.NearItem (_item, isInProximity);
					_wasJustFocused = false;
				}
			}
			return isInProximity;
		}

		public void Init() {
//			Debug.Log ("ProximityAgent[" + this.name + "]/Init");
			if(!_isInitialized) {
				_item = gameObject.GetComponent<Item> ();

				if (isTargetPlayer) {
					target = GameObject.Find ("player").transform;
				}
				_isInitialized = true;
			}
		}
		#endregion

		#region private methods
		private void Awake() {
			if (Game.Instance != null && Game.Instance.isSceneInitialized) {
				Init ();
			}
			EventCenter.Instance.OnSceneInitialized += this.OnSceneInitialized;
		}

		private void OnDestroy() {
			EventCenter ec = EventCenter.Instance;
			if (ec != null) {
				ec.OnSceneInitialized -= this.OnSceneInitialized;
			}
		}
		#endregion
	}
}
