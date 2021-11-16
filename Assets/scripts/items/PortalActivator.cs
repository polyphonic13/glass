using UnityEngine;
using Polyworks;

[System.Serializable]
public struct PortalSceneMap
{
    public string name;
    public SceneType target;
    public int section;
}

[RequireComponent(typeof(SubSceneSwitch))]
public class PortalActivator : CollectableItem
{

    public PortalSceneMap[] sceneMaps;


    public override void Actuate()
    {
        Game.Instance.SetFlag(PortalActivatorCharger.PORTAL_ACTIVATOR_COLLECTED, true);
        base.Actuate();
    }

    public override void Use()
    {
        bool isCharged = Game.Instance.GetFlag(PortalActivatorCharger.PORTAL_ACTIVATOR_CHARGED);
        Debug.Log("PortalActivator/Use, isCharged = " + isCharged + ", data.isUsable = " + data.isUsable);
        if (!data.isUsable)
        {
            EventCenter.Instance.AddNote(this.data.displayName + " is not usable at this time");
            return;
        }

        if (!isCharged)
        {
            EventCenter.Instance.AddNote(this.data.displayName + " is still recharging");
            return;
        }

        string currentScene = Game.Instance.gameData.currentScene;
        Debug.Log("  currentScene = " + currentScene);
        for (var i = 0; i < sceneMaps.Length; i++)
        {
            Debug.Log("  sceneMaps[ " + i + " ].name = " + sceneMaps[i].name);
            if (sceneMaps[i].name == currentScene)
            {
                _use(sceneMaps[i]);
                break;
            }
        }
    }

    private void _use(PortalSceneMap sceneMap)
    {
        base.Use();
        _initializeSceneSwitch(sceneMap);

        Game.Instance.SetFlag(PortalActivatorCharger.PORTAL_ACTIVATOR_CHARGED, false);
    }

    private void _initializeSceneSwitch(PortalSceneMap sceneMap)
    {
        Debug.Log("PortalActivator/_initializeSceneSwitch, scene = " + sceneMap.target + ", section = " + sceneMap.section);
        SubSceneSwitch sceneSwitch = GetComponent<SubSceneSwitch>();
        sceneSwitch.targetScene = sceneMap.target;
        sceneSwitch.targetSection = sceneMap.section;
        sceneSwitch.Actuate();
    }
}
