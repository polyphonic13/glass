using UnityEngine;
using Polyworks;

public class OnOffEmissionMaterial : Toggler
{
    public Material targetMaterial;

    public override void Toggle()
    {
        base.Toggle();
        toggleEmission();
    }

    private void toggleEmission()
    {
        if (targetMaterial == null)
        {
            return;
        }

        Log("OnOffEmissionMaterial[ " + this.name + " ]/toggleEmission");
        if (isOn)
        {
            Log("\tturning on");
            targetMaterial.EnableKeyword("_EMISSION");
            return;
        }
        Log("\tturning off");
        targetMaterial.DisableKeyword("_EMISSION");
    }

    private void Awake()
    {
        toggleEmission();
    }
}
