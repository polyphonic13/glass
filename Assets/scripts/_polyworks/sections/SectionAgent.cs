using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class SectionAgent : MonoBehaviour
	{
		public string sectionName; 

		private Item _item;

		public void OnSectionChanged(string section) {
			Debug.Log ("SectionAgent[" + this.name + "]/OnSectionChanged, section = " + section + ", sectionName = " + sectionName);
			if (section == sectionName) {
				Debug.Log (" enabling " + this.name);
				_item.enabled = true;
			} else {
				_item.enabled = false;
			}
		}

		private void Awake() {
			_item = this.gameObject.GetComponent<Item> ();
			_item.enabled = false;
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

