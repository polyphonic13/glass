using UnityEngine;
using Polyworks;

[System.Serializable]
public struct PortalSceneMap
{
    public string name;
    public SceneType targetScene;
    public int section;
}

public class PortalActivator : CollectableItem
{
    public PortalSceneMap[] sceneMaps;

    public Material OnMaterial;
    public Material OffMaterial;
    public Renderer ShellTop;
    public Renderer ShellBottom;

    private AnimationSwitch chargedAnimationSwitch;

    public void OnStringEvent(string type, string value)
    {
        if (type != PortalActivatorCharger.PORTAL_ACTIVATOR_CHARGED)
        {
            return;
        }
        setMaterials();
        chargedAnimationSwitch.Actuate();
    }

    public override void Actuate()
    {
        Game.Instance.SetFlag(PortalActivatorCharger.PORTAL_ACTIVATOR_COLLECTED, true);
        base.Actuate();
    }

    public override void Use()
    {
        bool isCharged = Game.Instance.GetFlag(PortalActivatorCharger.PORTAL_ACTIVATOR_CHARGED);
        Debug.Log("PortalActivator/Use, isCharged = " + isCharged);
        if (!data.isUsable)
        {
            return;
        }

        if (!isCharged)
        {
            EventCenter.Instance.AddNote(this.data.displayName + " is not usable at this time");
            return;
        }

        string currentScene = Game.Instance.CurrentSceneName;
        Debug.Log(" currentScene = " + currentScene);
        for (var i = 0; i < sceneMaps.Length; i++)
        {
            Debug.Log("  sceneMaps[ " + i + " ].name = " + sceneMaps[i].name);
            if (sceneMaps[i].name == currentScene)
            {
                Debug.Log("   found scene map at " + i);
                use(sceneMaps[i]);
                break;
            }
        }
    }

    private void use(PortalSceneMap sceneMap)
    {
        Game.Instance.SetFlag(PortalActivatorCharger.PORTAL_ACTIVATOR_CHARGED, false);

        initializeSceneSwitch(sceneMap);
    }

    private void initializeSceneSwitch(PortalSceneMap sceneMap)
    {
        Debug.Log("PortalActivator/initializeSceneSwitch, scene = " + sceneMap.targetScene + ", section = " + sceneMap.section);
        SceneSwitch sceneSwitch = GetComponent<SceneSwitch>();
        sceneSwitch.targetScene = sceneMap.targetScene;
        sceneSwitch.targetSection = sceneMap.section;
        sceneSwitch.Actuate();
    }

    private void setMaterials()
    {
        bool isCharged = Game.Instance.GetFlag(PortalActivatorCharger.PORTAL_ACTIVATOR_CHARGED);
        Material activeMaterial = (isCharged) ? OnMaterial : OffMaterial;
        ShellBottom.material = activeMaterial;
        ShellTop.material = activeMaterial;
    }

    private void Start()
    {
        setMaterials();
        chargedAnimationSwitch = GetComponent<AnimationSwitch>();
        EventCenter.Instance.OnStringEvent += OnStringEvent;
    }

    private void OnDestroy()
    {
        EventCenter eventCenter = EventCenter.Instance;
        if (eventCenter == null)
        {
            return;
        }
        eventCenter.OnStringEvent -= OnStringEvent;
    }
}
