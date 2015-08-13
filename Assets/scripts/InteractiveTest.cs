using UnityEngine;
using System.Collections;

public class InteractiveTest : MonoBehaviour {

	public GameObject interactiveIcon; 
	public float maxProximity = 5f;
	public float iconRotationSpeed = 10f;

	private bool _isJustChanged = false;
	private Quaternion _lookRotation;
	private Vector3 _direction;

	// Use this for initialization
	void Start () {
		if(interactiveIcon != null) {
			interactiveIcon.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		var target = Camera.main.transform;
		var distance = Vector3.Distance(this.transform.position, target.position);

		interactiveIcon.transform.LookAt(target);

		if(distance <= maxProximity) {
		    Vector3 dir = target.position - transform.position;
			// 1 if the same direction, -1 if opposite directions, 0 if perpendicular
		    if (Vector3.Dot(dir, transform.forward) == -1) {
				// player should be facing the item in order to activate its icon, aka opposite directions. 
				interactiveIcon.SetActive(true);
				_isJustChanged = true;
		    }
		} else {
			if(_isJustChanged) {
				interactiveIcon.SetActive(false);
				_isJustChanged = false;
			}
		}
	}
}
