﻿using UnityEngine;
using Polyworks;

[RequireComponent(typeof(ProximityAgent))]
public class CrystalReceptacle : Item
{

    public bool isStartEnabled = false;
    public string keyName;
    private GameObject _crystal;
    private bool _isUnlocked = false;
    private bool _isOpen = false;
    private Switch[] _switches;

    public void OnStringEvent(string type, string value)
    {
        Debug.Log("CrystalReceptacle[" + this.name + "]/OnStringEvent, type = " + type + ", value = " + value);
        if (type != CrystalKey.EVENT_NAME || value != keyName)
        {
            return;
        }

        Enable();
        ProximityAgent pa = GetComponent<ProximityAgent>();
        pa.SetFocus(true);
        _actuate();
    }

    public override void Actuate()
    {
        if (!isEnabled)
        {
            return;
        }

        if (!_isUnlocked)
        {
            EventCenter.Instance.AddNote("Crystal required to activate");
            return;
        }

        _actuate();
    }

    public override void Enable()
    {
        if (isEnabled)
        {
            return;
        }
        _isUnlocked = isEnabled = true;
        _crystal.SetActive(true);
        _addListeners();
        base.Enable();
    }

    public override void Disable()
    {
        if (!isEnabled)
        {
            return;
        }
        _isUnlocked = isEnabled = false;
        _crystal.SetActive(false);
        _removeListeners();
        base.Disable();
    }

    private void _actuate()
    {
        if (_switches == null)
        {
            return;
        }

        foreach (Switch s in _switches)
        {
            s.Actuate();
        }
    }

    private void _addListeners()
    {
        EventCenter eventCenter = EventCenter.Instance;
        if (eventCenter == null)
        {
            return;
        }
        eventCenter.OnStringEvent += this.OnStringEvent;
    }

    private void _removeListeners()
    {
        EventCenter eventCenter = EventCenter.Instance;
        if (eventCenter == null)
        {
            return;
        }
        eventCenter.OnStringEvent -= this.OnStringEvent;
    }

    private void Awake()
    {
        _crystal = this.transform.Find("crystal").gameObject;
        _crystal.SetActive(isStartEnabled);
        _isUnlocked = isEnabled = isStartEnabled;

        _switches = gameObject.GetComponents<Switch>();
    }

    private void OnDestroy()
    {
        _removeListeners();
    }

}
