using UnityEngine;
using System.Collections;

public class TargetTrigger : InteractiveItem {

	public TargetController target;
	public string disabledMessage = ""; 

	public override void Actuate() {
		if (this.IsEnabled) {
			if (!target.GetIsActive ()) {
				target.Actuate ();
			}
		} else if(disabledMessage != "") {
			EventCenter.Instance.AddNote (disabledMessage);
		}
	}
}
