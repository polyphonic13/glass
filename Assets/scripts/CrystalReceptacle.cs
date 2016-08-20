using UnityEngine;
using System.Collections;
using Polyworks; 

public class CrystalReceptacle : Item {

	public bool isStartEnabled = false;

	public TargetAgent target;
	public string keyName;

	private GameObject _crystal;

	private bool _isOpen = false;

	public void OnStringEvent(string type, string value) {
		Debug.Log ("CrystalReceptacle[" + this.name + "]/OnStringEvent, type = " + type + ", value = " + value);
		if (type == CrystalKey.EVENT_NAME && value == keyName) {
			isEnabled = true;
			_crystal.SetActive (true);
		}
	}

	public override void Actuate(Inventory inventory) {
		if (isEnabled) {
			if (!target.GetIsActive ()) {
				target.Actuate ();
			}
		} else {
			EventCenter.Instance.AddNote ("Crystal required to activate");
		}
	}

	private void Awake() {
		_crystal = this.transform.FindChild("crystal").gameObject;
		_crystal.SetActive (isStartEnabled);
		isEnabled = isStartEnabled;

		EventCenter ec = EventCenter.Instance;
		ec.OnStringEvent += this.OnStringEvent;
	}

	private void OnDestroy() {
		EventCenter ec = EventCenter.Instance;
		if (ec != null) {
			ec.OnStringEvent -= this.OnStringEvent;
		}
	}
}
