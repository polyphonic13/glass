using UnityEngine;

namespace Polyworks
{
    public class StringEventAgent : MonoBehaviour
    {
        public string eventType = "";
        public string eventValue = "";

        public bool isLogOn = false;

        private bool _isListenersAdded = false;

        public void OnStringEvent(string type, string value)
        {
            if (type != eventType && value != eventValue)
            {
                return;
            }
            Log("StringEventAgent[" + this.name + "]/OnStringEvent, type = " + type + ", eventType = " + eventType + ", value = " + value + ", eventValue = " + eventValue);
            SendMessage("Actuate", null, SendMessageOptions.DontRequireReceiver);
        }

        public void Enable()
        {
            _addListeners();
        }

        public void Disable()
        {
            _removeListeners();
        }

        private void Awake()
        {
            _addListeners();
        }

        private void OnDestroy()
        {
            _removeListeners();
        }

        private void _addListeners()
        {
            if (_isListenersAdded)
            {
                return;
            }

            _isListenersAdded = true;
            EventCenter ec = EventCenter.Instance;
            if (ec == null)
            {
                return;
            }
            ec.OnStringEvent += OnStringEvent;
        }

        private void _removeListeners()
        {
            if (!_isListenersAdded)
            {
                return;
            }
            _isListenersAdded = false;
            EventCenter ec = EventCenter.Instance;
            if (ec == null)
            {
                return;
            }
            ec.OnStringEvent -= OnStringEvent;
        }

        private void Log(string message)
        {
            if (!isLogOn)
            {
                return;
            }
            Debug.Log(message);
        }
    }
}
