namespace Polyworks
{
    using UnityEngine;
    using System.Collections;

    public class EventToggler : Toggler
    {
        public EventSwitch onEvent;
        public EventSwitch offEvent;

        public override void Toggle()
        {
            base.Toggle();
            EventSwitch evt = (isOn) ? onEvent : offEvent;
            if (isOn)
            {
                Log("EventToggler[" + this.name + "]/Toggle, evt = " + evt.type);
                evt.Actuate();
            }
        }
    }
}
