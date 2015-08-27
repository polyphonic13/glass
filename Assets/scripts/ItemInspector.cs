using UnityEngine;
using System.Collections;
using UnitySampleAssets.CrossPlatformInput;

public class ItemInspector : MonoBehaviour {

	public Transform target;

	public float distance = 5.0f;

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

	private Camera _camera;

	void Awake() {
		_camera = gameObject.GetComponent<Camera> ();
//		_camera.enabled = false;
//		Debug.Log("ItemInspector/Awake, camera = " + _camera);
	}
	
//	void Update() {
//		if (_camera.enabled) {
//			float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
//			float vertical = CrossPlatformInputManager.GetAxis("Vertical");
//
//
//		}
//	}

	void LateUpdate()
	{
		if (target) {
//			if (Input.GetMouseButton(1)) {
				float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
				float vertical = CrossPlatformInputManager.GetAxis("Vertical");
				velocityX += xSpeed * horizontal * 0.02f;
				velocityY += ySpeed * vertical * 0.02f;

//				velocityX += xSpeed * Input.GetAxis("Mouse X") * 0.02f;
//				velocityY += ySpeed * Input.GetAxis("Mouse Y") * 0.02f;
//			}
			
			rotationYAxis += velocityX;
			rotationXAxis -= velocityY;
			
			rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);
			Debug.Log("x axis = " + rotationXAxis + ", y axis = " + rotationYAxis);
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
