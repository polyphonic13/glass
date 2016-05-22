using UnityEngine;
using System.Collections;

public class CrystalReceptacle : InteractiveItem {

	public bool startEnabled = false;

	public TargetController target;
	public CrystalKey key;

	private GameObject _crystal;

	private bool _isOpen = false;

	void Awake() {
		_crystal = this.transform.FindChild("crystal").gameObject;
		EventCenter.Instance.OnCrystalKeyUsed += OnCrystalKeyUsed;
		_crystal.SetActive (startEnabled);
		this.isEnabled = startEnabled;
	}

	public void OnCrystalKeyUsed(string name) {
		if (key != null) {
			if (name == key.name) {
				this.isEnabled = true;
				_crystal.SetActive (true);
			}
		}
	}

	public override void Actuate() {
		if (this.isEnabled) {
			if (!target.GetIsActive ()) {
				target.Actuate ();
			}
		} else {
			EventCenter.Instance.AddNote ("Crystal required to activate");
		}
	}
}
