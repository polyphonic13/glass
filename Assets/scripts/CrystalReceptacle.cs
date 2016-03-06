using UnityEngine;
using System.Collections;

public class CrystalReceptacle : InteractiveItem {

	public bool startEnabled = false;

	public ArmatureParent target;
	public Transform bone;
	public AnimationClip unlockClip; 
	public AnimationClip closeClip;

	public CrystalKey key;

	public GameObject crystal;

	private bool _isOpen = false;

	void Awake() {
		EventCenter.Instance.OnCrystalKeyUsed += OnCrystalKeyUsed;
		crystal.SetActive (false);
		this.IsEnabled = startEnabled;
		if (startEnabled) {
			crystal.SetActive (true);
		}
	}

	public void OnCrystalKeyUsed(string name) {
		if (name == key.name) {
			this.IsEnabled = true;
			crystal.SetActive (true);
		}
	}

	public override void Actuate() {
		if (this.IsEnabled) {
			if (closeClip != null) {
				if (!_isOpen) {
					target.PlayAnimation (unlockClip.name, bone);
				} else {
					target.PlayAnimation (closeClip.name, bone);
				}
				_isOpen = !_isOpen;
			} else {
				target.PlayAnimation (unlockClip.name, bone);
			}
		} else {
			EventCenter.Instance.AddNote ("Crystal required to activate");
		}
	}
}
