namespace Polyworks {
	using UnityEngine;
	using System.Collections;

	public class MaterialToggleAgent : Toggler
	{
		public Material onMaterial;
		public Material offMaterial; 

		private Renderer _renderer;

		public override void Toggle ()
		{
			base.Toggle ();
			_toggleMaterial ();
		}

		private void Awake() {
			_renderer = this.gameObject.GetComponent<Renderer> ();
			_toggleMaterial ();
		}

		private void _toggleMaterial() {
			if (isOn) {
				_renderer.material = onMaterial;
			} else {
				_renderer.material = onMaterial;
			}
		}
	}
}