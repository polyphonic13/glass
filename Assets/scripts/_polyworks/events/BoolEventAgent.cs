
namespace Polyworks
{
    using UnityEngine;

    public class BoolEventAgent : EventAgent
    {
        public bool eventValue = false;

        public void OnBoolEvent(string type, bool value)
        {
            log("BoolEventAgent[" + this.name + "]/OnBoolEvent, type = " + type + ", EventType = " + EventType + ", value = " + value + ", eventValue = " + eventValue);
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
                ec.OnBoolEvent += OnBoolEvent;
            }
        }

        protected override void removeListeners()
        {
            EventCenter ec = EventCenter.Instance;
            if (ec != null)
            {
                ec.OnBoolEvent -= OnBoolEvent;
            }
        }
    }
}
