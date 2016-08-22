using UnityEngine;
using System.Collections;
using Polyworks; 

public class CrystalReceptacle : Item {

	public bool isStartEnabled = false;

	public string keyName;

	private GameObject _crystal;

	private bool _isUnlocked = false;
	private bool _isOpen = false;

	private AnimationSwitch _target;

	public void OnStringEvent(string type, string value) {
		if (type == CrystalKey.EVENT_NAME && value == keyName) {
			isEnabled = true;
			_isUnlocked = true;
			_crystal.SetActive (true);
			ProximityAgent pa = GetComponent<ProximityAgent> ();
			pa.SetFocus (true);
			_actuate ();
		}
	}

	public override void Actuate(Inventory inventory) {
		if (isEnabled) {
			if (_isUnlocked) {
				_actuate ();
			} else {
				EventCenter.Instance.AddNote ("Crystal required to activate");
			}
		}
	}

	private void Awake() {
		_target = GetComponent<AnimationSwitch> ();
		_crystal = this.transform.FindChild("crystal").gameObject;
		_crystal.SetActive (isStartEnabled);
		_isUnlocked = isEnabled = isStartEnabled;

		EventCenter ec = EventCenter.Instance;
		ec.OnStringEvent += this.OnStringEvent;
	}

	private void OnDestroy() {
		EventCenter ec = EventCenter.Instance;
		if (ec != null) {
			ec.OnStringEvent -= this.OnStringEvent;
		}
	}

	private void _actuate() {
		if (_target != null) {
			_target.Actuate ();
		}
	}
}
