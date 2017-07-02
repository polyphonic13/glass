namespace Polyworks {
	using UnityEngine;
	using System.Collections;
	using System;

	public class ExclusiveItemController : MonoBehaviour
	{
		public ExclusiveGameObject[] gameObjects;

		public string onEvent;
		public string offEvent;

		public void OnStringEvent(string type, string value) {
			Debug.Log ("ExclusiveItemController[" + this.name + "]/OnStringEvent, type = " + type + ", value = " + value);
			if (type == onEvent) {
				_setActiveByName (true, value);
			} else if (type == offEvent) {
				_setActiveByName (false, value);
			}
		}

		public void Init(bool isStartActive) {
			_setItemsActive (isStartActive);
		}

		private void Awake() {
			EventCenter.Instance.OnStringEvent += OnStringEvent;
		}

		private void _setActiveByName(bool isActive, string name) {
			int excludeIdx = _getIndexByName (name);
			Debug.Log ("ExclusiveItemAgent["+this.name+"]/_setActiveByName, excludeIdx = " + excludeIdx);
			if (excludeIdx > -1) {
				_setItemsActive (isActive, excludeIdx);
			}
		}

		private void _setItemsActive(bool isActive, int excludeIdx = -1) {
			for (int i = 0; i < gameObjects.Length; i++) {
				if (i == excludeIdx) {
					gameObjects [i].item.SetActive(isActive);
				} else {
					gameObjects [i].item.SetActive(!isActive);
				}
			}
		}

		private int _getIndexByName(string name) {
			for (int i = 0; i < gameObjects.Length; i++) {
				if (gameObjects [i].name == name) {
					return i;
				}
			}
			return -1;
		}

		private void OnDestroy() {
			EventCenter ec = EventCenter.Instance;
			if (ec != null) {
				ec.OnStringEvent -= OnStringEvent;
			}
		}
	}

	[Serializable]
	public struct ExclusiveGameObject {
		public string name;
		public GameObject item;
	}
}