using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;
using Polyworks;

public class InteractiveItem : MonoBehaviour {

	public Sprite Icon;
	public string itemName;

	public float interactDistance = 2f;
	
	protected Camera _mainCamera;

	private bool _wasJustFocused;

	void Awake() {
		Init();
	}

	public virtual void ItemUpdate() {
	}

	public virtual void Init() {
		isEnabled = true;

		_mainCamera = Camera.main;
	}

	public virtual void Actuate() {
		// Debug.Log (this.name + "/Actuate");
	}

	public Camera GetCamera() {
		return _mainCamera;
	}

	public void SetFocus(bool isFocused) {
//		Debug.Log (this.name + "/SetFocus, isFocused = " + isFocused + ", _wasJustFocused = " + _wasJustFocused);
		if (isFocused) {
			if (!_wasJustFocused) {
				EventCenter.Instance.NearInteractiveItem(this, true);
				_wasJustFocused = true;
			}
		} else if (_wasJustFocused) {
			EventCenter.Instance.NearInteractiveItem(this, false);
			_wasJustFocused = false;
		}
	}

	public bool CheckProximity() {
		bool isInProximity = false;
		var difference = Vector3.Distance(_mainCamera.transform.position, transform.position);
		if(difference < interactDistance) {
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
