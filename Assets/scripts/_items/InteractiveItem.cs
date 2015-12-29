using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

public class InteractiveItem : MonoBehaviour {

	public Sprite Icon;
	public Sprite Thumbnail;
	public string ItemName;

	public float _interactDistance = 3f;
	public string _containingRoom; 
	
	public bool IsRoomActive { get; set; } 
	public bool IsEnabled { get; set; }
	public bool IsFocused { get; set; }

	protected Camera MainCamera;

	private bool _wasJustFocused;

	void Awake() {
		Init();
	}

	void Update() {
		ItemUpdate ();
	}

	public virtual void ItemUpdate() {
		if(IsEnabled && IsRoomActive) {
//			Debug.Log("going to check proximity for: " + this.name);
			CheckProximity();
		}
	}

	public void Init() {
		IsEnabled = true;
		IsFocused = false;

		MainCamera = Camera.main;

		if(transform.tag == "persistentItem" || _containingRoom == "") {
			IsRoomActive = true;
		} else {
			IsRoomActive = false;
	
			var eventCenter = EventCenter.Instance;
			eventCenter.OnRoomEntered += OnRoomEntered;
			eventCenter.OnRoomExited += OnRoomExited;
		}
	}

	public virtual void Actuate() {
		Debug.Log (this.name + "/Actuate");
	}

	public string GetName() {
		return ItemName;
	}

	public Camera GetCamera() {
		return MainCamera;
	}

	public virtual void OnRoomEntered(string room) {
		if(room == _containingRoom) {
			Debug.Log("activating: " + room);
			IsRoomActive = true;
		}
	}

	public void OnRoomExited(string room) {
		if(room == _containingRoom) {
			Debug.Log("deactivating: " + room);
			IsRoomActive = false;
		}
	}

	public void SetFocus(bool isFocused) {
//		Debug.Log (this.name + "/SetFocus, isFocused = " + isFocused + ", _wasJustFocused = " + _wasJustFocused);
		if (isFocused) {
			if (!_wasJustFocused) {
				EventCenter.Instance.NearInteractiveItem(this, true);
				_wasJustFocused = true;
				IsFocused = true;
			}
		} else if (_wasJustFocused) {
			EventCenter.Instance.NearInteractiveItem(this, false);
			_wasJustFocused = false;
			IsFocused = false;
		}
	}

	public bool CheckProximity() {
		bool isInProximity = false;
		var difference = Vector3.Distance(MainCamera.transform.position, transform.position);
		if(difference < _interactDistance) {
			isInProximity = true;
//			Debug.Log("InteractiveItem["+this.name+"]/CheckProximity: " + isInProximity);
			EventCenter.Instance.NearInteractiveItem(this, isInProximity);
			_wasJustFocused = true;
		} else if(_wasJustFocused) {
			EventCenter.Instance.NearInteractiveItem(this, isInProximity);
			_wasJustFocused = false;
		}
//		Debug.Log(this.name + ", chcking proximity: " + isInProximity);

		return isInProximity;
	}
}
