using UnityEngine;
using System.Collections;

namespace Polyworks {

	public class Item : MonoBehaviour
	{
		public string displayName;
		public string description;

		public string prefabPath; 

		public bool isEnabled { get; set; }

		public int icon = 1;

		public virtual void Actuate() {}

		public virtual void Use() {}

		public virtual void SetEnabled(bool isEnabled) {
			this.isEnabled = isEnabled;
		}

		public virtual void Enable() {
//			Debug.Log ("Item[" + this.name + "]/Enable");
			this.SetEnabled (true);
		}

		public virtual void Disable() {
//			Debug.Log ("Item[" + this.name + "]/Disable");
			this.SetEnabled (false);
		}

		private void Awake() {
//			Debug.Log ("Item[" + this.name + "]/Awake, tag = " + transform.tag);
//			if (transform.tag == "persistent") {
//				Enable ();
//			}
		}
	}
}

