﻿using UnityEngine;
using System.Collections;

namespace Polyworks
{
    public class SwitchController : Item
    {
        #region public members
        public bool isLocked = false;
        public bool isLockMessageDisplayed = true;
        public bool isAutoActuatedOnUnlock = false;
        public bool isNoteAddedOnUnlock = true;

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
            //			Debug.Log ("SwitchController[" + this.name + "]/OnStringEvent, type " + type + ", eventType = " + eventType);
            //			Debug.Log(" value = " + value + ", eventValue = " + eventValue);
            if (type == eventType && value == eventValue)
            {
                //				Debug.Log (" is a MATCH");
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
            Debug.Log("SwitchController[" + this.name + "]/Actuate, isLocked = " + isLocked + ", _switches = " + _switches.Length);
            if (!isLocked)
            {
                base.Actuate();
                _actuate();
            }
            else if (isLockMessageDisplayed)
            {
                if (customLockedMessage == "")
                {
                    EventCenter.Instance.AddNote("The " + this.displayName + " is locked");
                }
                else
                {
                    EventCenter.Instance.AddNote(customLockedMessage);
                }
            }
        }
        #endregion

        #region private methods
        private void _actuate()
        {
            if (_switches != null)
            {
                for (int i = 0; i < _switches.Length; i++)
                {
                    if (_switches[i] != null)
                    {
                        _switches[i].Actuate();
                    }
                }
            }
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
