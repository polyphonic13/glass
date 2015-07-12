using UnityEngine;
using System.Collections;

public class SceneChanger : MonoBehaviour {

	public string targetScene;
	public int targetRoom;

	private bool activated { get; set; }

	void Awake() {
		activated = true;
	}

	void OnTriggerEnter(Collider tgt) {
		if(activated) {
			Debug.Log("scene changer trigger, tgt.tag = " + tgt.gameObject.tag);
			if(tgt.gameObject.tag == "Player") {
				GameControl.Instance.changeScene(targetScene, targetRoom);
			}
		}
	}

	public void setActive(bool active) {
		activated = active;
	}
}
