using UnityEngine;

public class SceneChangeTrigger : MonoBehaviour {

	public string _targetScene;
	public int _targetRoom;

	private bool isActive { get; set; }

	void Awake() {
		isActive = false;
	}

	void OnTriggerEnter(Collider tgt) {
//		Debug.Log ("SceneChangeTrigger/OnTriggerEnter, tgt = " + tgt.gameObject.tag + ", isActive = " + isActive);
		if(isActive) {
//			Debug.Log("scene changer trigger, tgt.tag = " + tgt.gameObject.tag);
			if(tgt.gameObject.tag == "Player") {
//				GameController.Instance.ChangeScene(_targetScene, _targetRoom);
			}
		}
	}

	public void SetActive(bool active) {
//		Debug.Log ("SceneChangeTrigger/SetActive, active = " + active);
		isActive = active;
	}

}
