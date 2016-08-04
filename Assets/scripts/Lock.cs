using UnityEngine;
using System.Collections;

public class Lock : MonoBehaviour {

	public ArmatureParent target;
	public Transform bone;
	public AnimationClip unlockClip; 

	public CrystalKey crystal;

	void Awake() {
//		EventCenter.Instance.OnCrystalKeyUsed += OnCrystalKeyUsed;
	}
	
	public void OnCrystalKeyUsed(string name) {
		if (name == crystal.name) {
			target.PlayAnimation (unlockClip.name, bone);
		}
	}
}
