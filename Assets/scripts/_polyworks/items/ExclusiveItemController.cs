namespace Polyworks {
	using UnityEngine;
	using System.Collections;
	using System;

	public class ExclusiveItemController : MonoBehaviour
	{
		public ExclusiveGameObject[] gameObjects;

		public string[] eventTypes; 

		public void OnStringEvent(string type, string value) {
			for(int i = 0; i < eventTypes.Length; i++) {
				if (eventTypes [i] == type) {
					_deactiveOthers (value);
				}
			}
		}

		public void Init(bool isStartActive) {
			_setItemsActive (isStartActive);
		}

		private void Awake() {
			EventCenter.Instance.OnStringEvent += OnStringEvent;
		}

		private void _deactiveOthers(string name) {
			int activeIdx = _getIndexByName (name);
			_setItemsActive (false, activeIdx);
		}

		private void _setItemsActive(bool isActive, int excludeIdx = -1) {
			for (int i = 0; i < gameObjects.Length; i++) {
				if (i != excludeIdx) {
					gameObjects [i].item.SetActive(isActive);
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