using UnityEngine;

public class SceneChanger : MonoBehaviour {

	public string _targetScene;
	public int _targetRoom;

	private bool activated { get; set; }

	void Awake() {
		activated = true;
	}

	void OnTriggerEnter(Collider tgt) {
		if(activated) {
//			Debug.Log("scene changer trigger, tgt.tag = " + tgt.gameObject.tag);
			if(tgt.gameObject.tag == "Player") {
				GameControl.Instance.ChangeScene(_targetScene, _targetRoom);
			}
		}
	}

	public void SetActive(bool active) {
		activated = active;
	}
}
