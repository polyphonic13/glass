using UnityEngine;
using System.Collections;
using Polyworks; 

public class CrystalReceptacle : Item {

	public bool isStartEnabled = false;

	public TargetController target;
	public string keyName;

	private GameObject _crystal;
	private CrystalKey _key; 

	private bool _isOpen = false;

	public void OnSceneInitialized(string scene) {
		_key = GameObject.Find (keyName).GetComponent<CrystalKey>();
	}

	public void OnStringEvent(string type, string value) {
		if (type == CrystalKey.EVENT_NAME) {
			if (_key != null && value == _key.name) {
				this.isEnabled = true;
				_crystal.SetActive (true);
			}
		}
	}

	public override void Actuate(Inventory inventory) {
		if (this.isEnabled) {
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
		this.isEnabled = isStartEnabled;

		EventCenter ec = EventCenter.Instance;
		ec.OnSceneInitialized += this.OnSceneInitialized;
		ec.OnStringEvent += this.OnStringEvent;
	}

	private void OnDestroy() {
		EventCenter ec = EventCenter.Instance;
		ec.OnSceneInitialized -= this.OnSceneInitialized;
		ec.OnStringEvent -= this.OnStringEvent;
	}
}
