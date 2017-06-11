namespace Polyworks {
	using UnityEngine; 

	public class GameObjectActiveToggler: Toggler {
		public GameObject target; 
		
		public override void Toggle() {
			base.Toggle();
			
			target.SetActive(isOn);
		}

		private void Awake() {
			target.SetActive(isOn);
		}
	}
}