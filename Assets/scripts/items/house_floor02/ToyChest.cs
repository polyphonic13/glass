using UnityEngine;
using Polyworks;

public class ToyChest : Item
{
    public static string RABBIT_HUNT_ADD_EVENT = "rabbitHuntToyAdded";
    public static string RABBIT_HUNT_COMPLETE_EVENT = "rabbitHuntCompleted";
    public static string LID_COLLIDER_EVENT = "toggleToyChestOpen";

    public GameObject[] collectedToys;
    public string completedMessage;

    private EventCenter eventCenter;
    private int _collected = 0;
    private int _expected = 2;
    private bool _isLidOpen = false;

    #region handlers
    public void OnStringEvent(string type, string value)
    {
        Log("ToyChest/OnStringEvent, type = " + type + ", value = " + value + ", _isLidOpen = " + _isLidOpen);
        if (type == RABBIT_HUNT_ADD_EVENT)
        {
            if (!_isLidOpen)
            {
                // dispatch an event to open the lid first
                eventCenter.InvokeStringEvent("open_toychest_lid", "");
                // toggle once before event received
                _isLidOpen = true;
            }
            for (int i = 0; i < collectedToys.Length; i++)
            {
                Log("  collectedToys[" + i + "].name = " + collectedToys[i].name);
                if (collectedToys[i].name == value)
                {
                    collectedToys[i].SetActive(true);
                    _collected++;
                    break;
                }
            }
            Log(" _collected = " + _collected + ", _expected = " + _expected);
            if (_collected == _expected)
            {
                eventCenter.AddNote(completedMessage);
                eventCenter.InvokeStringEvent(RABBIT_HUNT_COMPLETE_EVENT, "");
            }
            return;
        }

        if (type != LID_COLLIDER_EVENT)
        {
            return;
        }
        _isLidOpen = !_isLidOpen;
        Log(" is lid open now: " + _isLidOpen);
    }
    #endregion

    #region public methods
    public override void Enable()
    {
        base.Enable();
        eventCenter.OnStringEvent += OnStringEvent;
    }

    public override void Disable()
    {
        base.Disable();
        eventCenter.OnStringEvent -= OnStringEvent;
    }
    #endregion

    #region private methods
    private void Awake()
    {
        for (int i = 0; i < collectedToys.Length; i++)
        {
            collectedToys[i].SetActive(false);
        }

        eventCenter = EventCenter.Instance;
    }

    private void OnDestroy()
    {
        if (eventCenter == null)
        {
            return;
        }
        eventCenter.OnStringEvent -= OnStringEvent;
    }
    #endregion
}
