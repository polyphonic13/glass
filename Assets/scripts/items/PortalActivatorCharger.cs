using UnityEngine;
using Polyworks;

public class PortalActivatorCharger : MonoBehaviour
{
    public static readonly string PORTAL_ACTIVATOR_COLLECTED = "isPortalActivatorCollected";
    public static readonly string PORTAL_ACTIVATOR_CHARGED = "isPortalActivatorCharged";
    public static readonly string USABLE_MESSAGE = "The mysterious device opened and started glowing";

    public float secondsToCharge = 5.0f;

    private bool isCharged = false;

    private float isChargedCounter;

    public void OnSceneInitialized(string scene)
    {
        isCharged = Game.Instance.GetFlag(PORTAL_ACTIVATOR_CHARGED);
    }

    private void Awake()
    {
        isChargedCounter = secondsToCharge;

        EventCenter.Instance.OnSceneInitialized += OnSceneInitialized;
    }

    private void FixedUpdate()
    {
        if (isCharged)
        {
            return;
        }
        // Debug.Log ("PortalActivator: " + Time.deltaTime + ", isChargedCounter = " + isChargedCounter);
        isChargedCounter -= Time.deltaTime;

        if (isChargedCounter > 0)
        {
            return;
        }

        EventCenter eventCenter = EventCenter.Instance;
        Game game = Game.Instance;

        isCharged = true;
        isChargedCounter = 0;

        game.SetFlag(PORTAL_ACTIVATOR_CHARGED, isCharged);

        eventCenter.InvokeStringEvent(PORTAL_ACTIVATOR_CHARGED);


        bool isCollected = game.GetFlag(PORTAL_ACTIVATOR_COLLECTED);
        // Debug.Log ("PortalActivator now charged, isCollected = " + isCollected);
        if (!isCollected)
        {
            return;
        }
        eventCenter.AddNote(USABLE_MESSAGE);
    }

    private void OnDestroy()
    {
        EventCenter eventCenter = EventCenter.Instance;
        if (eventCenter == null)
        {
            return;
        }
        eventCenter.OnSceneInitialized -= OnSceneInitialized;
    }
}

