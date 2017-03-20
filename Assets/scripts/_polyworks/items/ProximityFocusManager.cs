namespace Polyworks {
	using System;
	using UnityEngine;
	
    [RequireComponent(typeof (RaycastAgent))]
	public class ProximityFocusManager : MonoBehaviour {
		
		private RaycastAgent _raycastAgent; 
		
		public void OnSetTarget(GameObject target) {
			_updateTarget(target, true);
		}
		
		public void OnClearTarget(GameObject target) {
			_updateTarget(target, false);
		}
		
		private void Awake() {
			_raycastAgent = this.gameObject.GetComponent<RaycastAgent>();
			if(_raycastAgent != null) {
				_raycastAgent.OnSetTarget += this.OnSetTarget;
				_raycastAgent.OnClearTarget += this.OnClearTarget;
			}
		}
		
		private void OnDestroy() {
			if(_raycastAgent != null) {
				_raycastAgent.OnSetTarget -= this.OnSetTarget;
				_raycastAgent.OnClearTarget -= this.OnClearTarget;
			}
		}
		
		private void _updateTarget(GameObject target, bool isFocused) {
			ProximityAgent proximityAgent = target.GetComponent<ProximityAgent>();
			if(proximityAgent != null) {
				proximityAgent.setFocus(isFocused);
			}
		}
	}
}