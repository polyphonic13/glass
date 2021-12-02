namespace Polyworks
{
    public class SwitchController : Item
    {
        #region public members
        public bool isLocked = false;
        public bool isLockMessageDisplayed = true;
        public bool isAutoActuatedOnUnlock = false;
        public bool isNoteAddedOnUnlock = true;
        public bool IsDisabledOnUse = false;

        public string customLockedMessage = "";

        public string eventType = "";
        public string eventValue = "";
        #endregion

        #region private members
        private Switch[] _switches;
        private bool _isInSection = false;
        #endregion

        #region handlers
        public void OnStringEvent(string type, string value)
        {
            Log("SwitchController[" + this.name + "]/OnStringEvent, type " + type + ", eventType = " + eventType);
            Log(" value = " + value + ", eventValue = " + eventValue);
            if (type == eventType && value == eventValue)
            {
                Log(" is a MATCH");
                if (isLocked)
                {
                    isLocked = false;
                    if (isNoteAddedOnUnlock)
                    {
                        EventCenter.Instance.AddNote("The " + this.displayName + " was unlocked");
                    }
                    if (isAutoActuatedOnUnlock)
                    {
                        Actuate();
                    }
                }
            }
        }
        #endregion

        #region public methods
        public override void Actuate()
        {
            Log("SwitchController[" + this.name + "]/Actuate, isLocked = " + isLocked + ", isEnabled = " + isEnabled + ", _switches = " + _switches.Length);
            if (isLocked)
            {
                showLockedMessaged();
                return;
            }

            _actuate();
        }
        #endregion

        #region private methods
        private void showLockedMessaged()
        {
            if (!isLockMessageDisplayed)
            {
                return;
            }

            string message = (customLockedMessage != "") ? customLockedMessage : "The " + this.displayName + " is locked";
            EventCenter.Instance.AddNote(message);

        }

        private void _actuate()
        {
            if (!this.isEnabled)
            {
                return;
            }

            base.Actuate();

            if (_switches == null)
            {
                return;
            }

            for (int i = 0; i < _switches.Length; i++)
            {
                if (_switches[i] != null)
                {
                    _switches[i].Actuate();
                }
            }

            if (!IsDisabledOnUse)
            {
                return;
            }
            isEnabled = false;
            EventCenter.Instance.InvokeItemDisabled();
        }

        private void initEnable()
        {
            if (transform.tag != "persistent")
            {
                return;
            }
            Enable();
        }

        private void Awake()
        {
            initEnable();
            _switches = gameObject.GetComponents<Switch>();
            Log("SwitchController[ " + this.name + " ]/Awake, _switches.Length = " + _switches.Length);

            if (!isLocked || eventType == "")
            {
                return;
            }
            EventCenter.Instance.OnStringEvent += OnStringEvent;
        }

        private void OnDestroy()
        {
            EventCenter ec = EventCenter.Instance;
            if (ec != null)
            {
                EventCenter.Instance.OnStringEvent -= OnStringEvent;
            }
        }
        #endregion
    }
}
