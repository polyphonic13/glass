using UnityEngine;
using System.Collections;
using UnitySampleAssets.CrossPlatformInput;

public class ItemInspector : MonoBehaviour {

	public const int INSPECTOR_LAYER = 15;

	public Transform target;

	public float distance = 4.0f;

	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;
	
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
	
	public float distanceMin = .5f;
	public float distanceMax = 15f;
	
	public float smoothTime = 2f;
	
	private float rotationYAxis = 0.0f;
	private float rotationXAxis = 0.0f;
	
	private float velocityX = 0.0f;
	private float velocityY = 0.0f;

	private Transform _previousParent;
	private Vector3 _previousPosition;
	private int _previousLayer;

	private Camera _camera;

	private static ItemInspector _instance;
	private ItemInspector() {}
	
	public static ItemInspector Instance {
		get {
			if(_instance == null) {
				_instance = GameObject.FindObjectOfType(typeof(ItemInspector)) as ItemInspector;      
			}
			return _instance;
		}
	}
	
	public void AddTarget(Transform tgt) {
		target = tgt;

		_previousParent = target.parent.transform;
		_previousPosition = target.position;
		_previousLayer = target.gameObject.layer;

		target.parent = transform.parent;
		target.gameObject.layer = INSPECTOR_LAYER;

		Vector3 position = new Vector3 (transform.position.x + distance, transform.position.y, transform.position.z);
		target.transform.position = position;
		transform.rotation = Quaternion.LookRotation (transform.position - target.transform.position);

		_camera.enabled = true;
	}

	public void RemoveTarget() {
		target.parent = _previousParent;
		target.position = _previousPosition;
		target.gameObject.layer = _previousLayer;

		target = null;
		_camera.enabled = false;
	}

	void Awake() {
		_camera = gameObject.GetComponent<Camera> ();
		_camera.enabled = false;
	}
	
	void LateUpdate() {
		if (target) {
			if(CrossPlatformInputManager.GetButtonDown("Cancel")) {
				EventCenter.Instance.InspectItem(false, target.name);
			} else {
				float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
				float vertical = CrossPlatformInputManager.GetAxis("Vertical");
				velocityX += xSpeed * horizontal * 0.01f;
				velocityY += ySpeed * vertical * 0.01f;

				rotationYAxis += velocityX;
				rotationXAxis -= velocityY;
				
				rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);
				Quaternion fromRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
				Quaternion toRotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);
				Quaternion rotation = toRotation;

				Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
				Vector3 position = rotation * negDistance + target.position;
				
				transform.rotation = rotation;
				transform.position = position;
				
				velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime * smoothTime);
				velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * smoothTime);
			}
		}
	}
	
	public static float ClampAngle(float angle, float min, float max) {
		if (angle < -360F) {
			angle += 360F;
		}
		if (angle > 360F) {
			angle -= 360F;
		}
		return Mathf.Clamp(angle, min, max);
	}
}
