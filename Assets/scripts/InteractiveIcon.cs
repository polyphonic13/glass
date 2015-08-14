using UnityEngine;

public class InteractiveIcon : MonoBehaviour {

	public GameObject interactiveIcon; 
	public float maxProximity = 5f;
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
		if(_element == null || _element.IsEnabled) {
			var target = Camera.main.transform;
			var distance = Vector3.Distance(this.transform.position, target.position);
		
			interactiveIcon.transform.LookAt(target);

			if(distance <= maxProximity) {
			    Vector3 dir = target.position - transform.position;
				// 1 if the same direction, -1 if opposite directions, 0 if perpendicular
			    if(Vector3.Dot(dir, transform.forward) < 0) {
					_turnOnIcon();
				} else {
					_turnOffIcon();
				}
			} else {
				_turnOffIcon();
			}
		}
	}
	
	void _turnOnIcon() {
		interactiveIcon.SetActive(true);
		_isJustChanged = true;
	}
	
	void _turnOffIcon() {
		if(_isJustChanged) {
			interactiveIcon.SetActive(false);
			_isJustChanged = false;
		}
	}
}
