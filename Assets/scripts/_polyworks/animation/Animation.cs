using UnityEngine;

namespace Polyworks {
	public class Animation: MonoBehaviour, IPauseResumable {
		
		public bool isActive = false; 
		
		public virtual void Play() {
			isActive = true;
		}
		
		public virtual void Pause() {
			isActive = false;
		}
		
		public virtual void Resume() {
			isActive = true;
		}
		
		public virtual bool GetIsActive() {
			return isActive;
		}
	}
}