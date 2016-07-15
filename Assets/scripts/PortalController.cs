using UnityEngine;
using System.Collections;

public class PortalController : MonoBehaviour {

	public bool startActivated;

	public ParticleSystem up;
	public ParticleSystem down; 

	public ArmatureParent shellAnimator; 

	private bool _isActive; 

	private SceneChangeTrigger sceneChanger;

	private const string OPEN_ANIMATION_CLIP = "portal00_open";

	void Awake () {
		sceneChanger = GameObject.Find("collider_back").GetComponent<SceneChangeTrigger> ();
//		// Debug.Log ("sceneChanger = " + sceneChanger);

		if (!startActivated) {
			_toggleParticleSystemStart (false);
		}
	}

	public void Activate() {
		if (!_isActive) {
//			// Debug.Log ("activating portal");
			shellAnimator.PlayAnimation (OPEN_ANIMATION_CLIP);
			_toggleParticleSystemStart (true);
			_isActive = true;
		}
	}

	public void Deactivate() {
		if (_isActive) {
			_toggleParticleSystemStart (false);
			_isActive = false;
		}
	}

	private void _toggleParticleSystemStart(bool enable) {
		up.enableEmission = enable;
		down.enableEmission = enable;
		sceneChanger.SetActive (enable);
	}
}
