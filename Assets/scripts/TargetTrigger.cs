using UnityEngine;
using System.Collections;
using Polyworks;

public class TargetTrigger : Item {

	public TargetController target;
	public string disabledMessage = ""; 

	public override void Actuate() {
		if (this.isEnabled) {
			if (!target.GetIsActive ()) {
				target.Actuate ();
			}
		} else if(disabledMessage != "") {
			EventCenter.Instance.AddNote (disabledMessage);
		}
	}
}
