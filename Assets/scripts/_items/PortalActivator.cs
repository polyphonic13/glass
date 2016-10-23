using UnityEngine;
using System.Collections;
using Polyworks;

[System.Serializable]
public struct PortalSceneMap {
	public string name;
	public string target;
	public int section;
}

public class PortalActivator : CollectableItem {

	public PortalSceneMap[] sceneMaps; 

	public override void Actuate() {
		Game.Instance.SetFlag (PortalActivatorCharger.PORTAL_ACTIVATOR_COLLECTED, true);
		base.Actuate ();
	}
	
	public override void Use () {
		bool isCharged = Game.Instance.GetFlag (PortalActivatorCharger.PORTAL_ACTIVATOR_CHARGED);
//		Debug.Log ("PortalActivator/Use, isCharged = " + isCharged);
		if (data.isUsable && isCharged) {
			string currentScene = Game.Instance.gameData.currentScene;
			for (var i = 0; i < sceneMaps.Length; i++) {
				if (sceneMaps [i].name == currentScene) {
					_use (sceneMaps [i]);
					break;
				}
			} 
		} else {
			EventCenter.Instance.AddNote (this.data.displayName + " is not usable at this time");
		}
	}

	private void _use(PortalSceneMap sceneMap) {
		_initializeSceneSwitch (sceneMap);

		Game.Instance.SetFlag (PortalActivatorCharger.PORTAL_ACTIVATOR_CHARGED, false);
		base.Use ();
	}

	private void _initializeSceneSwitch(PortalSceneMap sceneMap) {
//		Debug.Log ("PortalActivator/_initializeSceneSwitch, scene = " + sceneMap.target + ", section = " + sceneMap.section);
		SceneSwitch sceneSwitch = GetComponent<SceneSwitch> ();
		sceneSwitch.targetScene = sceneMap.target;
		sceneSwitch.targetSection = sceneMap.section;
	}

}
