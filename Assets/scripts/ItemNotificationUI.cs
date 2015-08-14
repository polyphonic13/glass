using UnityEngine;
using System.Collections;

public class ItemNotificationUI : MonoBehaviour {

	private GameObject _iconInteract;
	private GameObject _iconMagnify;
	private GameObject _iconGrab;
	private GameObject _iconPush; 

	void Start() {
		_iconInteract = transform.Find("icon_interact").gameObject;
		_iconMagnify = transform.Find("icon_magnify").gameObject;
		_iconGrab = transform.Find("icon_grab").gameObject;
		_iconPush = transform.Find("icon_push").gameObject;
    }
	
	void Update() {
	
	}
}
