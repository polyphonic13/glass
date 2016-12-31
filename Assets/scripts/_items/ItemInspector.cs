using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Polyworks;

public class ItemInspector : MonoBehaviour, IInputControllable {

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
	public float maxZoom = 4f;
	public float minZoom = -4f;

	private float _horizontal = 0;
	private float _vertical = 0;

	private bool _cancel = false; 
	private bool _zoomIn = false; 
	private bool _zoomOut = false; 

	private float _rotationYAxis = 0.0f;
	private float _rotationXAxis = 0.0f;
	
	private float _velocityX = 0.0f;
	private float _velocityY = 0.0f;

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

	public void OnInspectItem(bool isInspecting, string itemName) {
		if (isInspecting) {
			CollectableItem item = Game.Instance.GetPlayerInventory ().GetItem (itemName);
			AddTarget (item.transform, item.data.displayName, item.data.description);
		} else {
			RemoveTarget ();
		}
	}

	public void SetInput(InputObject input) {

	}

	public void SetHorizontal(float horizontal) {
		_horizontal = horizontal;
	}

	public void SetVertical(float vertical) {
		_vertical = vertical;
	}

	public void SetZoomIn(bool zoomIn) {
		_zoomIn = zoomIn;
	}

	public void SetZoomOut(bool zoomOut) {
		_zoomOut = zoomOut;
	}

	public void SetCancel(bool cancel) {
		_cancel = cancel;
	}

	public void AddTarget(Transform item, string itemName, string itemDescription) {
		Debug.Log ("AddTarget, item = " + item);
		_item = item;

//		_previousParent = _item.parent.transform;
//		_previousPosition = _item.position;
//		_previousLayer = _item.gameObject.layer;

		_item.parent = transform.parent;
//		_item.gameObject.layer = INSPECTOR_LAYER;
		Utilities.Instance.ChangeLayers(_item.gameObject, INSPECTOR_LAYER);

		Vector3 position = new Vector3 (transform.position.x + distance, transform.position.y, transform.position.z);
		_item.transform.position = position;
		_itemName.text = itemName;
		_itemDescription.text = itemDescription;
		_uiCamera.enabled = true;
		_camera.enabled = true;
	}

	public void RemoveTarget() {
		_item.parent = _previousParent;
		_item.position = _previousPosition;
//		_item.gameObject.layer = _previousLayer;
		Utilities.Instance.ChangeLayers (_item.gameObject, _previousLayer);

		_item = null;
		_camera.enabled = false;
		_camera.fieldOfView = _initialFieldOfView;
		_currentZoom = 0;

		_uiCamera.enabled = false;
		_itemName.text = "";
		_itemDescription.text = "";

		_rotationYAxis = 0.0f;
		_rotationXAxis = 0.0f;
		
		_velocityX = 0.0f;
		_velocityY = 0.0f;

		transform.position = _initialPosition;
		transform.rotation = _initialRotation;
		
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

		EventCenter.Instance.OnInspectItem += OnInspectItem;
	}
	
	void LateUpdate() {
		// based on: http://answers.unity3d.com/questions/463704/smooth-orbit-round-object-with-adjustable-orbit-ra.html
		if (_item) {
//			Debug.Log("LateUpdate, horizontal = " + _horizontal + ", vertical = " + _vertical);
			if(_cancel) {
				EventCenter.Instance.InspectItem(false, _item.name);
			} else if(_zoomIn) {
				if(_currentZoom < maxZoom) {
					_camera.fieldOfView += zoomAmount;
					_currentZoom++;
				}
			} else if(_zoomOut) {
				if(_currentZoom > minZoom) {
					_camera.fieldOfView -= zoomAmount;
					_currentZoom--;
				}
			} else {
				_velocityX = xSpeed * _horizontal * 0.01f;
				_velocityY = ySpeed * _vertical * 0.01f;
				
				_rotationYAxis += _velocityX;
				_rotationXAxis -= _velocityY;
				
				_rotationXAxis = ClampAngle(_rotationXAxis, yMinLimit, yMaxLimit);
				Quaternion fromRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
				Quaternion toRotation = Quaternion.Euler(_rotationXAxis, _rotationYAxis, 0);
				Quaternion rotation = toRotation;
				
				Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
				Vector3 position = rotation * negDistance + _item.position;
				
				transform.rotation = rotation;
				transform.position = position;
				
				_velocityX = Mathf.Lerp(_velocityX, 0, Time.deltaTime * smoothTime);
				_velocityY = Mathf.Lerp(_velocityY, 0, Time.deltaTime * smoothTime);
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
