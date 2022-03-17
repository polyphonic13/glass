namespace Polyworks
{
    using UnityEngine;

    public class IntEventAgent : EventAgent
    {
        public int eventValue = -1;

        public void OnIntEvent(string type, int value)
        {
            log("IntEventAgent[" + this.name + "]/OnIntEvent, type = " + type + ", EventType = " + EventType + ", value = " + value + ", eventValue = " + eventValue);
            if (type == EventType && value == eventValue)
            {
                SendMessage("Actuate", null, SendMessageOptions.DontRequireReceiver);
            }
        }

        protected override void addListeners()
        {
            EventCenter ec = EventCenter.Instance;
            if (ec != null)
            {
                ec.OnIntEvent += OnIntEvent;
            }
        }

        protected override void removeListeners()
        {
            EventCenter ec = EventCenter.Instance;
            if (ec != null)
            {
                ec.OnIntEvent -= OnIntEvent;
            }
        }
    }
}
