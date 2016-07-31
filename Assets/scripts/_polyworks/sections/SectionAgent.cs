using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class SectionAgent : MonoBehaviour
	{
		public int section = -1; 

		private Item _item;

		public void OnSectionChanged(int s) {
			if (section == s) {
				_item.isEnabled = true;
			} else {
				_item.isEnabled = false;
			}
		}

		private void Awake() {
			_item = this.gameObject.GetComponent<Item> ();
			if (_item != null) {
				if (section > -1) {
					_item.isEnabled = false;
				}
				EventCenter.Instance.OnSectionChanged += this.OnSectionChanged;	
			}
		}

		private void Destroy() {
			if (_item != null) {
				EventCenter.Instance.OnSectionChanged -= this.OnSectionChanged;	
			}
		}
	}
}

