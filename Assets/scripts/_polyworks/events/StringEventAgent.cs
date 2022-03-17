namespace Polyworks
{
    using UnityEngine;

    public class StringEventAgent : EventAgent
    {
        public string eventValue = "";

        private bool isListenersAdded = false;

        public void OnStringEvent(string type, string value)
        {
            log("StringEventAgent[" + this.name + "]/OnStringEvent, type = " + type + ", EventType = " + EventType + ", value = " + value + ", eventValue = " + eventValue);
            if (type != EventType && value != eventValue)
            {
                return;
            }
            SendMessage("Actuate", null, SendMessageOptions.DontRequireReceiver);
        }

        protected override void addListeners()
        {
            if (isListenersAdded)
            {
                return;
            }

            isListenersAdded = true;
            EventCenter ec = EventCenter.Instance;

            if (ec == null)
            {
                return;
            }
            ec.OnStringEvent += OnStringEvent;
        }

        protected override void removeListeners()
        {
            if (!isListenersAdded)
            {
                return;
            }

            isListenersAdded = false;
            EventCenter ec = EventCenter.Instance;

            if (ec == null)
            {
                return;
            }
            ec.OnStringEvent -= OnStringEvent;
        }
    }
}
