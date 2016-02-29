using UnityEngine;
using System.Collections;

public class CrystalReceptacle : MonoBehaviour {

	public ArmatureParent target;
	public Transform bone;
	public AnimationClip unlockClip; 

	public CrystalKey key;

	public GameObject crystal;

	void Awake() {
		EventCenter.Instance.OnCrystalKeyUsed += OnCrystalKeyUsed;
		crystal.SetActive (false);
	}

	public void OnCrystalKeyUsed(string name) {
		if (name == key.name) {
			target.PlayAnimation (unlockClip.name, bone);
			crystal.SetActive (true);
		}
	}
}
