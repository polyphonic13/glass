namespace Polyworks {
	using UnityEngine;
	using System.Collections;

	public class PlayerHead : MonoBehaviour {

		public float rayLength = 2f;
		
		public bool isUsingProximityAgent = true;
		public bool isCheckingTags = true; 
		
		public string tag1 = "interactive";
		public string tag2 = "persistent"; 
		
		private string _targetName;
		private GameObject _target; 
		
		public delegate void HitTargetHandler(GameObject target);
		public event HitTargetHandler OnSetTarget;
		public event HitTargetHanlder OnClearTarget;
		
		private void Update () {
			_checkRayCast();
		}

		private void _setTarget(Raycast hit) {
			_targetName = hit.transform.name;
			_target = hit.transform.gameObject; 
			
			if(OnSetTarget != null) {
				OnSetTarget(_target);
			}
		}
		
		private void _clearTarget() {
			if(OnClearTarget != null) {
				OnClearTarget(_target);
			}
			
			_targetName = "";
			_target = null;
		}

		private void _checkRayCast() {
			RaycastHit hit;
			if (Physics.Raycast (this.transform.position, this.transform.forward, out hit, rayLength)) {
				if (hit.transform != this.transform) {
					if(isCheckingTags) {
						if(hit.transform.tag == tag1 || hit.transform.tag == tag2) {
							if (hit.transform.name != _targetName) {
								_setTarget(hit);
							}
						} else {
							_clearTarget();
						}
					} else {
						_setTarget(hit);
					}
				} else {
					_clearTarget();
				}
			} else {
				_clearTarget();
			}
		}

	}
}
