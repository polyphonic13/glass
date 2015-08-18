using UnityEngine;

public class InteractiveIcon : MonoBehaviour {

	public GameObject interactiveIcon; 
	public float iconRotationSpeed = 10f;

	private bool _isJustChanged = false;
	private InteractiveElement _element; 
	
	void Start() {
		_element = gameObject.GetComponent<InteractiveElement>();
		
		if(interactiveIcon != null) {
			interactiveIcon.SetActive(false);
		}
	}
	
	void Update() {
		if(_element.IsEnabled) {
			interactiveIcon.transform.LookAt(_element.getCamera().transform);

			if(_element.CheckProximity()) {

//			    Vector3 dir = target.position - transform.position;
				// 1 if the same direction, -1 if opposite directions, 0 if perpendicular
//			    if(Vector3.Dot(dir, transform.forward) < 0) {
					_turnOnIcon();
//				} else {
//					_turnOffIcon();
//				}
			} else if(_isJustChanged){
				Debug.Log ("just changed");
				_turnOffIcon();
			}
		}
	}
	
	void _turnOnIcon() {
		Debug.Log("turning icon " + this.name + " on");
		interactiveIcon.SetActive(true);
		_isJustChanged = true;
	}
	
	void _turnOffIcon() {
		interactiveIcon.SetActive(false);
		_isJustChanged = false;
	}
}
