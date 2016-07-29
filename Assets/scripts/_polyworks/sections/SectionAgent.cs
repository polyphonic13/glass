using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class SectionAgent : MonoBehaviour
	{
		public string sectionName; 

		private Item _item;

		public void OnSectionChanged(string section) {
			if (section == sectionName) {
				_item.enabled = true;
			} else {
				_item.enabled = false;
			}
		}

		private void Awake() {
			_item = this.gameObject.GetComponent<Item> ();
			if (_item != null) {
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

