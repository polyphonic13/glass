using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnitySampleAssets.CrossPlatformInput;

public class ItemInspector : MonoBehaviour {

	public const int INSPECTOR_LAYER = 15;

	public float distance = 2.0f;

	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;
	
	public float yMinLimit = -361f;
	public float yMaxLimit = 361f;
	
	public float distanceMin = .5f;
	public float distanceMax = 15f;
	
	public float smoothTime = 2f;

	public float zoomAmount = 15f;
	public float maxZoom = 3f;
	public float minZoom = -2f;

	private float rotationYAxis = 0.0f;
	private float rotationXAxis = 0.0f;
	
	private float velocityX = 0.0f;
	private float velocityY = 0.0f;

	private Transform _item;
	private Transform _previousParent;
	private Vector3 _previousPosition;
	private int _previousLayer;

	private Camera _uiCamera; 
	private Camera _camera;
	private float _initialFieldOfView				;
	private Quaternion _initialRotation;
	private Vector3 _initialPosition;

	private int _currentZoom;

	private Text _itemName;
	private Text _itemDescription;

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
	
	public void AddTarget(Transform item, string itemName, string itemDescription) {

		_item = item;

		transform.position = _initialPosition;
		transform.rotation = _initialRotation;

		_previousParent = _item.parent.transform;
		_previousPosition = _item.position;
		_previousLayer = _item.gameObject.layer;

		_item.parent = transform.parent;
		_item.gameObject.layer = INSPECTOR_LAYER;

		Vector3 position = new Vector3 (transform.position.x + distance, transform.position.y, transform.position.z);
		_item.transform.position = position;
		transform.rotation = Quaternion.LookRotation (transform.position - _item.transform.position);
		_itemName.text = itemName;
		_itemDescription.text = itemDescription;
		_uiCamera.enabled = true;
		_camera.enabled = true;
	}

	public void RemoveTarget() {
		_item.parent = _previousParent;
		_item.position = _previousPosition;
		_item.gameObject.layer = _previousLayer;

		_item = null;
		_camera.enabled = false;
		_camera.fieldOfView = _initialFieldOfView;
		_currentZoom = 0;
		Debug.Log ("current rotation = " + transform.rotation + ", init = " + _initialRotation);
		transform.position = _initialPosition;
		transform.rotation = _initialRotation;

		_uiCamera.enabled = false;
		_itemName.text = "";
		_itemDescription.text = "";

	}

	void Awake() {
		_camera = gameObject.GetComponent<Camera> ();
		_camera.enabled = false;
		_initialFieldOfView = _camera.fieldOfView;
		_initialRotation = transform.rotation;
		_initialPosition = transform.position;

		Transform uiCam = transform.parent.transform.Find ("item_inspector_ui_camera");
		_uiCamera = uiCam.GetComponent<Camera> ();
		_uiCamera.enabled = false;
		_itemName = uiCam.transform.Find ("inspector_ui/text_name").GetComponent<Text>();
		_itemName.text = "";
		_itemDescription = uiCam.transform.Find ("inspector_ui/text_description").GetComponent<Text>();
		_itemDescription.text = "";
	}
	
	void LateUpdate() {
		if (_item) {
			if(CrossPlatformInputManager.GetButtonDown("Cancel")) {
				EventCenter.Instance.InspectItem(false, _item.name);
			} else if(CrossPlatformInputManager.GetButtonDown("Fire1")) {
				if(_currentZoom < maxZoom) {
					_camera.fieldOfView += zoomAmount;
					_currentZoom++;
				}
			} else if(CrossPlatformInputManager.GetButtonDown("Fire2")) {
				if(_currentZoom > minZoom) {
					_camera.fieldOfView -= zoomAmount;
					_currentZoom--;
				}
			} else {
				float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
				float vertical = CrossPlatformInputManager.GetAxis("Vertical");
				velocityX = xSpeed * horizontal * 0.01f;
				velocityY = ySpeed * vertical * 0.01f;

				rotationYAxis += velocityX;
				rotationXAxis -= velocityY;
				
				rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);
				Quaternion fromRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
				Quaternion toRotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);
				Quaternion rotation = toRotation;

				Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
				Vector3 position = rotation * negDistance + _item.position;
				
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
