using UnityEngine;
using System.Collections;

public class SceneChanager : MonoBehaviour {

	[SerializeField] private string _levelTarget;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision tgt) {
		Debug.Log("scene changer collision, tgt.tag = " + tgt.gameObject.tag);
		if(tgt.gameObject.tag == "Player") {
			Application.LoadLevel(_levelTarget);
		}
	}

	void OnTriggerEnter(Collider tgt) {
		Debug.Log("scene changer trigger, tgt.tag = " + tgt.gameObject.tag);
		if(tgt.gameObject.tag == "Player") {
			Application.LoadLevel(_levelTarget);
		}
	}
}
