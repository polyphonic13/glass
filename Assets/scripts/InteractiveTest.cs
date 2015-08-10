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
			interactiveIcon.SetActive(true);
			_isJustChanged = true;
//			_direction = (targetPosition- interactiveIcon.transform.position).normalized;
//			
//			//create the rotation we need to be in to look at the target
//			_lookRotation = Quaternion.LookRotation(_direction);
//			
//			//rotate us over time according to speed until we are in the required rotation
//			interactiveIcon.transform.rotation = Quaternion.Slerp(interactiveIcon.transform.rotation, _lookRotation, Time.deltaTime * iconRotationSpeed);
		} else {
			if(_isJustChanged) {
				interactiveIcon.SetActive(false);
				_isJustChanged = false;
			}
		}
	}
}
