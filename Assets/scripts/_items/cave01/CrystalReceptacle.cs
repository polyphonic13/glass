using UnityEngine;
using System.Collections;
using Polyworks; 

public class CrystalReceptacle : Item {

	public bool isStartEnabled = false;

	public string keyName;

	private GameObject _crystal;

	private bool _isUnlocked = false;
	private bool _isOpen = false;

	private Switch[] _switches;

	public void OnStringEvent(string type, string value) {
		Debug.Log ("CrystalReceptacle[" + this.name + "]/OnStringEvent, type = " + type + ", value = " + value);
		if (type == CrystalKey.EVENT_NAME && value == keyName) {
			isEnabled = true;
			_isUnlocked = true;
			_crystal.SetActive (true);
			ProximityAgent pa = GetComponent<ProximityAgent> ();
			pa.SetFocus (true);
			_actuate ();
		}
	}

	public override void Actuate() {
		if (isEnabled) {
			if (_isUnlocked) {
				_actuate ();
			} else {
				EventCenter.Instance.AddNote ("Crystal required to activate");
			}
		}
	}

	public override void Enable() {
		if (!isEnabled) {
			base.Enable ();
			_addListeners ();
		}
	}

	public override void Disable() {
		base.Disable ();
		_removeListeners ();
	}

	private void Awake() {
		_crystal = this.transform.FindChild("crystal").gameObject;
		_crystal.SetActive (isStartEnabled);
		_isUnlocked = isEnabled = isStartEnabled;

		_switches = gameObject.GetComponents<Switch> ();
	}

	private void OnDestroy() {
		_removeListeners ();
	}

	private void _actuate() {
		if (_switches != null) {
			for (int i = 0; i < _switches.Length; i++) {
				if (_switches [i] != null) {
					_switches [i].Actuate ();
				}
			}
		}
	}

	private void _addListeners() {
		EventCenter ec = EventCenter.Instance;
		if (ec != null) {
			ec.OnStringEvent += this.OnStringEvent;
		}
	}

	private void _removeListeners() {
//		Debug.Log ("CrystalReceptacle[" + this.name + "]/_removeListeners");
		EventCenter ec = EventCenter.Instance;
		if (ec != null) {
			ec.OnStringEvent -= this.OnStringEvent;
		}
	}
}
