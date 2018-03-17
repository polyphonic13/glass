namespace Polyworks {
	using UnityEngine;
	using System.Collections;

	public class EventToggler : Toggler
	{
		public EventSwitch onEvent;
		public EventSwitch offEvent;

		public override void Toggle ()
		{
			EventSwitch evt;

			base.Toggle ();
			if (isOn) {
				evt = onEvent;
			} else {
				evt = offEvent;
			}
			Log ("EventToggler[" + this.name + "]/Toggle, evt = " + evt.type);

			evt.Actuate ();
		}
	}

}