using UnityEngine;
using Polyworks;

public class Flashlight : CollectableItem
{
    public const string COLLECTED = "isFlashlightCollected";
    private Light bulb;

    public void OnCollectFlashlight()
    {
        Log("Flashlight[" + this.name + "]/OnCollectFlashlight");
        this.data.isCollected = true;
    }

    public void OnEnableFlashlight()
    {
        Log("Flashlight[" + this.name + "]/OnEnableFlashlight, isCollected = " + this.data.isCollected);
        if (this.data.isCollected)
        {
            bulb.enabled = !bulb.enabled;
        }
    }

    public override void Actuate()
    {
        Log("Flashlight/Actuate");

        Game.Instance.SetFlag(COLLECTED, true);
        EventCenter ec = EventCenter.Instance;
        ec.CollectFlashight();
        ec.NearItem(this, false);
        ec.AddNote(this.displayName + " added");

        _removeListeners();
        Destroy(this.gameObject);
    }

    private void Awake()
    {
        bulb = gameObject.GetComponent<Light>();
        bulb.enabled = false;

        EventCenter ec = EventCenter.Instance;
        ec.OnCollectFlashlight += OnCollectFlashlight;
        ec.OnEnableFlashlight += OnEnableFlashlight;
    }

    private void OnDestroy()
    {
        _removeListeners();
    }

    private void _removeListeners()
    {
        EventCenter ec = EventCenter.Instance;
        if (ec != null)
        {
            ec.OnCollectFlashlight -= OnCollectFlashlight;
            ec.OnEnableFlashlight -= OnEnableFlashlight;
        }
    }
}
