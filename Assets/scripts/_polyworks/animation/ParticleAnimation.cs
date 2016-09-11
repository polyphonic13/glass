using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class ParticleAnimation : AnimationAgent
	{
		public ParticleSystem[] systems;
		public bool isStartActive; 

		public override void Play(string clip = "") {
			base.Play();
			_toggleSystems (true);
		}

		public override void Pause() {
			base.Pause ();
			_toggleSystems (false);
		}

		public override void Resume() {
			base.Resume ();
			_toggleSystems (true);
		}

		public override bool GetIsActive() {
			return isActive;
		}
	
		private void Awake() {
			_toggleSystems (isStartActive);
		}

		private void _toggleSystems(bool enable) {
			for (var i = 0; i < systems.Length; i++) {
				systems [i].enableEmission = enable;
			}
		}
	}
}

