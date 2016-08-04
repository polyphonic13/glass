using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class SectionAgent : Reaction
	{
		public int section = -1; 

		public void OnSectionChanged(int s) {
			if (section == s) {
				this.gameObject.SendMessage ("Enable");
			} else {
				this.gameObject.SendMessage ("Disable");
			}
		}

		private void Awake() {
			EventCenter.Instance.OnSectionChanged += this.OnSectionChanged;	
		}

		private void Destroy() {
			Debug.Log("SectionAgaent["+this.name+"]/Destroy");

			EventCenter.Instance.OnSectionChanged -= this.OnSectionChanged;	
		}
	}
}

