using UnityEngine;
using Polyworks;

public class OnOffLight : Toggler
{

    public Light bulb;

    public override void Toggle()
    {
        base.Toggle();
        ToggleTarget(isOn);
    }

    public override void ToggleTarget(bool turnOn)
    {
        isOn = turnOn;
        Log("ToggleTarget[" + this.name + "], isOn = " + isOn);
        bulb.enabled = isOn;
    }

    void Awake()
    {
        ToggleTarget(isOn);
    }
}
