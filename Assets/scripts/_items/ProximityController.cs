using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;
using Polyworks;

public class ProximityController : MonoBehaviour {

	public float interactDistance = 2f;
	public Transform target;

	private bool _wasJustFocused;
	private Item _item;
	
	public void SetFocus(bool isFocused) {
		if (isFocused) {
			if (!_wasJustFocused) {
				EventCenter.Instance.NearInteractiveItem(_item, true);
				_wasJustFocused = true;
			}
		} else if (_wasJustFocused) {
			EventCenter.Instance.NearInteractiveItem(_item, false);
			_wasJustFocused = false;
		}
	}

	public bool Check() {
		bool isInProximity = false;
		var difference = Vector3.Distance(target.position, transform.position);
		if(difference < interactDistance) {
			isInProximity = true;
			EventCenter.Instance.NearInteractiveItem(_item, isInProximity);
			_wasJustFocused = true;
		} else if(_wasJustFocused) {
			EventCenter.Instance.NearInteractiveItem(_item, isInProximity);
			_wasJustFocused = false;
		}
		return isInProximity;
	}
	
	private void Awake() {
		_item = gameObject.GetComponent<Item> ();
	}
}
