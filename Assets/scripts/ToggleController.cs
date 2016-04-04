using UnityEngine;
using System.Collections;

public class ToggleController : MonoBehaviour {

	public Toggler[] _togglers; 

	public virtual void Actuate() {
//		Debug.Log ("ToggleController[" + this.name + "]/Actuate");
		for (int i = 0; i < _togglers.Length; i++) {
			_togglers [i].Toggle ();
		}
	}
}
