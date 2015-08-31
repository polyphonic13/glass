using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

public class InteractiveItem : MonoBehaviour {

	public Sprite Thumbnail;
	public string ItemName;

	public float _interactDistance = 3f;
	public string _containingRoom; 
	
	public bool IsRoomActive { get; set; } 
	public bool IsEnabled { get; set; }

	protected Camera MainCamera;

	private bool _wasJustInProximity;

	void Awake() {
		Init();
	}

	void Update() {
		ItemUpdate ();
	}

	public virtual void ItemUpdate() {
		if(IsEnabled) {
			CheckProximity();
		}
	}

	public void Init() {
		IsEnabled = true;

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
			IsRoomActive = true;
		}
	}

	public void OnRoomExited(string room) {
		if(room == _containingRoom) {
			IsRoomActive = false;
		}
	}

	public bool CheckProximity() {
		bool isInProximity = false;
		var difference = Vector3.Distance(MainCamera.transform.position, transform.position);
		if(difference < _interactDistance) {
			isInProximity = true;
//			Debug.Log("InteractiveItem["+this.name+"]/CheckProximity: " + isInProximity);
			EventCenter.Instance.NearInteractiveItem(this, true);
			_wasJustInProximity = true;
		} else if(_wasJustInProximity) {
			EventCenter.Instance.NearInteractiveItem(this, false);
			_wasJustInProximity = false;
		}

		return isInProximity;
	}
}
