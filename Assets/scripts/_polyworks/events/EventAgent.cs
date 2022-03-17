namespace Polyworks
{
    using UnityEngine;

    public abstract class EventAgent : MonoBehaviour
    {
        public string EventType;
        public bool IsLogOn;

        public void Enable()
        {
            addListeners();
        }

        public void Disable()
        {
            removeListeners();
        }

        protected void log(string message)
        {
            if (!IsLogOn)
            {
                return;
            }
            Debug.Log(message);
        }

        protected virtual void addListeners()
        {

        }

        protected virtual void removeListeners()
        {

        }

        private void Awake()
        {
            addListeners();
        }

        private void OnDestroy()
        {
            removeListeners();
        }
    }
}
