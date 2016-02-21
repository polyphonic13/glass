using UnityEngine;
using System.Collections;

public class PortalController : MonoBehaviour {

	public bool startActivated;

	public ParticleSystem up;
	public ParticleSystem down; 

	private bool _isActive; 

	void Awake () {
		if (!startActivated) {
			_toggleParticleSystemStart (false);
		}
	}

	public void Activate() {
		if (!_isActive) {
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
	}
}
