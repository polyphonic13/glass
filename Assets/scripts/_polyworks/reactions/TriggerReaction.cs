using UnityEngine;
using System.Collections;

namespace Polyworks {
	public class TriggerReaction : Reaction
	{
		public virtual void OnTriggerEnter(Collider tgt) {
			Execute();
		}
	}
}

