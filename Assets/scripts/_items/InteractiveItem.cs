using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;
using Polyworks;

public class InteractiveItem : Item {

	public Sprite Icon;
	public string itemName;

	public float _interactDistance = 3f;

	public bool IsFocused { get; set; }

	protected Camera MainCamera;

	private bool _wasJustFocused;

	void Awake() {
		Init();
	}

	public virtual void ItemUpdate() {
	}

	public virtual void Init() {
		IsEnabled = true;
		IsFocused = false;

		MainCamera = Camera.main;
	}

	public virtual void Actuate() {
		Debug.Log (this.name + "/Actuate");
	}

	public string GetName() {
		return itemName;
	}

	public Camera GetCamera() {
		return MainCamera;
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
