using UnityEngine;
using System.Collections;

public class TargetAgent : MonoBehaviour {

	public bool isEnabled { get; set; }

	public virtual void Actuate() {

	}

	public virtual bool GetIsActive() {
		return false;
	}

	public virtual void Pause() {

	}

	public virtual void Resume() {

	}

	public virtual void SetEnabled(bool isEnabled) {
		this.isEnabled = isEnabled;
	}

	public virtual void Enable() {
		this.SetEnabled (true);
		if (GetIsActive ()) {
			this.Resume ();
		}
	}

	public virtual void Disable() {
		this.SetEnabled (false);
		if (GetIsActive ()) {
			this.Pause ();
		}
	}

}
