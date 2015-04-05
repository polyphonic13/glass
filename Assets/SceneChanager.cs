using UnityEngine;
using System.Collections;

public class SceneChanager : MonoBehaviour {

	[SerializeField] private string _targetScene;
	[SerializeField] private int _targetRoom;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision tgt) {
		Debug.Log("scene changer collision, tgt.tag = " + tgt.gameObject.tag);
		if(tgt.gameObject.tag == "Player") {
			Application.LoadLevel(_targetScene);
		}
	}

	void OnTriggerEnter(Collider tgt) {
		Debug.Log("scene changer trigger, tgt.tag = " + tgt.gameObject.tag);
		if(tgt.gameObject.tag == "Player") {
			GameControl.instance.targetRoom = _targetRoom;
			Application.LoadLevel(_targetScene);
			var player = GameObject.Find("Player");
			player.transform.position = GameControl.instance.getStartingPosition();
		}
	}
}
