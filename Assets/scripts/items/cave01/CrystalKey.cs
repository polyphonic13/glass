using Polyworks;
using UnityEngine;
[RequireComponent(typeof(ProximityAgent))]
public class CrystalKey : CollectableItem
{
    public static string EVENT_NAME = "crystalKeyUsed";

    public override void Use()
    {
        EventCenter.Instance.InvokeStringEvent(EVENT_NAME, this.name);
    }
}
