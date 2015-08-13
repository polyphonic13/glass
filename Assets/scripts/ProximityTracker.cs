using UnityEngine;
using System.Collections;

public class ProximityTracker : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    Ray ray = new Ray(transform.position, transform.forward);
	    RaycastHit hit;
	    if(Physics.Raycast(ray, out hit, 100)){
			if(hit.transform.tag === 'interactive') {
				
			}
	    }
	}
}
