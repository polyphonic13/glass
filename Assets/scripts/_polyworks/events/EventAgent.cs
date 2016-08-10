using UnityEngine;

namespace Polyworks {
	public class EventAgent: Monobehaviour {
		public string eventName = ""; 
		
		public void Actuate() {
			EventCenter ec = EventCenter.Instance;
			if(ec != null) {
				ec.invokeStringEvent(eventName, this.name);
			}
		}
	}
}